using Application.Services.Common;

namespace Application.Services.Assembler.Implementations;

public sealed class HackAssembler(
    IParser parser,
    ITranslator translator,
    ISymbolTable symbolTable,
    IFileManager fileManager) : IAssembler
{
    private int _variableRegisterCount = 16; // address of the first register used for storing variable values
    
    private async Task FirstPass(string source)
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

    private async Task SecondPass(string source)
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

            var parsedInstruction = parser.ParseAssembly(instruction);
            output.AddRange(translator.TranslateParsedAssembly(parsedInstruction));
        }

        await fileManager.WriteToFileAsync(source, output.ToArray());
    }

    private static bool IsSymbol(string line)
    {
        var instructionA = line.StartsWith('@');
        var notDigit = !char.IsDigit(line[1]);
        return instructionA && notDigit;
    }

    private static bool IsInvalidLine(string? line)
    {
        return line is null || string.IsNullOrWhiteSpace(line) || line.StartsWith("//");
    }
    
    public async Task Assemble(string source)
    {
        await FirstPass(source);
        await SecondPass(source);
    }
}