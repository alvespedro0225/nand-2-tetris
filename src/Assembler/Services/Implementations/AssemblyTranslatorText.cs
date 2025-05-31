using System.Text;

namespace Assembler.Services.Implementations;

public sealed class TextTranslator : ITranslator
{
    private readonly Dictionary<string, string> _computationCodes = new()
    {
        { "0", "0101010" },
        { "1", "0111111" },
        { "D", "0001100" },
        { "A", "0110000" },
        { "M", "1110000" },
        { "-1", "0111010" },
        { "!D", "0001101" },
        { "!A", "0110001" },
        { "!M", "1110001" },
        { "-D", "0001111" },
        { "-A", "0110011" },
        { "-M", "1110011" },
        { "D+1", "0011111" },
        { "A+1", "0110111" },
        { "M+1", "1110111" },
        { "D-1", "0001110" },
        { "A-1", "0110010" },
        { "M-1", "1110010" },
        { "D+A", "0000010" },
        { "D+M", "1000010" },
        { "D-A", "0010011" },
        { "D-M", "1010011" },
        { "A-D", "0000111" },
        { "M-D", "1000111" },
        { "D&A", "0000000" },
        { "D&M", "1000000" },
        { "D|A", "0010101" },
        { "D|M", "1010101" }
    };

    private readonly Dictionary<string, string> _jumpCodes = new()
    {
        {string.Empty, "000"},
        { "JGT", "001" },
        { "JEQ", "010" },
        { "JGE", "011" },
        { "JLT", "100" },
        { "JNE", "101" },
        { "JLE", "110" },
        { "JMP", "111" } 
    };
    public byte[] TranslateParsedAssembly(char[][] assembly)
    {
        return assembly.Length switch
        {
            1 => Encoding.UTF8.GetBytes(TranslateInstructionA(assembly[0])),
            3 => Encoding.UTF8.GetBytes(TranslateInstructionC(assembly[0], assembly[1], assembly[2])),
            _ => throw new Exception("Invalid assembly format")
        };
    }
    
    private string TranslateInstructionA(char[] assembly)
    {
        if (int.TryParse(assembly, out int instructionNumber))
            return $"{instructionNumber:b16}\n";

        throw new Exception("Invalid A instruction");
    }

    private string TranslateInstructionC(char[] computation, char[] destination, char[] jump)
    {
        var builder = new StringBuilder("111"); // start of C instruction binary
        builder.Append(TranslateComputation(new string(computation)));
        builder.Append(TranslateDestination(destination));
        builder.Append(TranslateJump(new string(jump)));
        builder.Append("\n");
        return builder.ToString();
    }

    private string TranslateDestination(ReadOnlySpan<char> destination)
    {
        Span<char> bits = new char[3];
        bits.Fill('0');

        if (destination.ContainsAny(['a', 'A']))
            bits[0] = '1';
        
        if (destination.ContainsAny(['d', 'D']))
            bits[1] = '1';
        
        if (destination.ContainsAny(['m', 'M']))
            bits[2] = '1';
        
        return bits.ToString();
    }

    private string TranslateJump(string? jump)
    {
        jump ??= string.Empty;
        
        if (!_jumpCodes.TryGetValue(jump, out var jumpBinary))
            throw new Exception("Invalid syntax");

        return jumpBinary;
    }

    private string TranslateComputation(string? computation)
    {
        computation ??= string.Empty;
        
        if (!_computationCodes.TryGetValue(computation, out var computationBinary))
            throw new Exception("Invalid syntax");

        return computationBinary;
    }
}