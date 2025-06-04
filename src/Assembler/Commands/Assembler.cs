using Assembler.Services;
using Cocona;
using Core.Services.Common;

namespace Assembler.Commands;

public static class Assembler
{
    private static int _variableRegisterCount = 16; // address of the first register used for storing variable values
    
    public static void AddAssemblerCommands(this CoconaApp app)
    {
        app.AddCommand(StartProgram);
    }

    private static async Task StartProgram(
        IFileService fileService,
        IParser parser,
        ITranslator translator,
        ISymbolTable symbolTable)
    {
        var files = fileService.GetFiles();

        while (files.MoveNext())
        {
            using var file = files.Current;
            await FirstPass(file, fileService,symbolTable);
            file.BaseStream.Position = 0;
            await SecondPass(file, fileService, parser, symbolTable, translator);
        }
        
    }
    
    private static async Task FirstPass(
        StreamReader file,
        IFileService fileService,
        ISymbolTable symbolTable)
    {
        var lineCounter = 0;
        while (!file.EndOfStream)
        {
            var line = await file.ReadLineAsync();
            line = line?.Trim();
            if (IsInvalidLine(line))
                continue;

            if (line!.StartsWith('('))
            {
                // adds the symbol without () to the table
                symbolTable.AddSymbol($"{fileService.FileName}.{line[1..^1]}", lineCounter);
                continue;
            }
            
            lineCounter++;
        }
    }

    private static async Task SecondPass(
        StreamReader file,
        IFileService fileService, 
        IParser parser, 
        ISymbolTable symbolTable,
        ITranslator translator)
    {
        List<byte> output = [];
        while (!file.EndOfStream)
        {
            var line = await file.ReadLineAsync();
            line = line?.Trim();
            
            if (IsInvalidLine(line) || line!.StartsWith('(')) // need to skip definitions on second pass
                continue;

            var instruction = line.Split("//")[0].Trim(); // removes comments and whitespace
            
            if (IsSymbol(line))
            {
                if (!symbolTable.TryGetSymbol($"{fileService.FileName}.{line[1..]}", out var symbolValue))
                {
                    symbolTable.AddSymbol($"{fileService.FileName}.{line[1..]}", _variableRegisterCount);
                    symbolValue = _variableRegisterCount++;
                }
                
                instruction = $"@{symbolValue}";
            }

            var parsedInstruction = parser.Parse(instruction);
            output.AddRange(translator.Translate(parsedInstruction));
        }

        await fileService.WriteToFileAsync(output.ToArray(), ".hack");
    }

    private static bool IsSymbol(string line)
    {
        var instructionA = line.StartsWith('@');
        var notDigit = !char.IsDigit(line[1]);
        return instructionA && notDigit;
    }

    private static bool IsInvalidLine(string? line)
    {
        return string.IsNullOrWhiteSpace(line) || line.StartsWith('/');
    }
}