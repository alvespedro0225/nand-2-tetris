using Application.Services.Assembler;
using Application.Services.Common;
using Cocona;

namespace Assembler.Commands;

public static class Assembler
{
    private static string _source = null!;
    private static int _variableRegisterCount = 16; // address of the first register used for storing variable values
    
    public static void AddAssemblerCommands(this CoconaApp app, string source)
    {
        _source = source;
        app.AddCommand(StartProgram);
    }
    public static async Task StartProgram(
        IFileManager fileManager,
        IParser parser,
        ITranslator translator,
        ISymbolTable symbolTable)
    {
        await FirstPass(_source, fileManager, symbolTable);
        await SecondPass(_source, fileManager, parser, symbolTable, translator);
    }
    
    private static async Task FirstPass(string source, IFileManager fileManager, ISymbolTable symbolTable)
    {
        using var reader = fileManager.ReadFile(source);
        var lineCounter = 0;
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            line = line?.Trim();
            if (IsInvalidLine(line))
                continue;

            if (line!.StartsWith('('))
            {
                // adds the symbol without () to the table
                symbolTable.AddSymbol(line[1..^1], lineCounter);
                continue;
            }
            
            lineCounter++;
        }
    }

    private static async Task SecondPass(
        string source, 
        IFileManager fileManager, 
        IParser parser, 
        ISymbolTable symbolTable,
        ITranslator translator)
    {
        using var reader = fileManager.ReadFile(source);
        List<byte> output = [];
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            line = line?.Trim();
            
            if (IsInvalidLine(line) || line!.StartsWith('(')) // need to skip definitions on second pass
                continue;

            var instruction = line.Split("//")[0].Trim(); // removes comments and whitespace
            
            if (IsSymbol(line))
            {
                if (!symbolTable.TryGetSymbol(line[1..], out var symbolValue))
                {
                    symbolTable.AddSymbol(line[1..], _variableRegisterCount);
                    symbolValue = _variableRegisterCount++;
                }
                
                instruction = $"@{symbolValue}";
            }

            var parsedInstruction = parser.Parse(instruction);
            output.AddRange(translator.Translate(parsedInstruction));
        }

        await fileManager.WriteToFileAsync(source, output.ToArray(), ".hack");
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