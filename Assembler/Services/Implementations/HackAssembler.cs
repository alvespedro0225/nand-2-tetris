using System.Text;

namespace Assembler.Services.Implementations;

public sealed class HackAssembler(
    IParser parser,
    ITranslator translator,
    ISymbolTable symbolTable) : IHackAssembler
{
    private int _variableRegisterCount = 16;
    
    private async Task FirstPass(string source)
    {
        using var reader = File.OpenText(source);
        var lineCounter = 0;
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            line = line?.Trim();
            if (IsInvalidLine(line))
                continue;

            if (line!.StartsWith('('))
            {
                symbolTable.AddSymbol(line[1..^1], lineCounter);
                continue;
            }
            
            lineCounter++;
        }
    }

    private async Task SecondPass(string source)
    {
        using var reader = File.OpenText(source);
        var stringOutput = new StringBuilder();
 
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
            stringOutput.AppendLine(translator.TranslateParsedAssembly(parsedInstruction));
        }

        var destinationFile = Path.ChangeExtension(source, ".hack");
        await using var writer = File.OpenWrite(destinationFile);
        await writer.WriteAsync(Encoding.UTF8.GetBytes(stringOutput.ToString()));
        Console.WriteLine($"{destinationFile}");
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