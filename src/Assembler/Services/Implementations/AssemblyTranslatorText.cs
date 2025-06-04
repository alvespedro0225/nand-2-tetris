using System.Text;
using Core.Exceptions;

namespace Assembler.Services.Implementations;

public sealed class AssemblyTranslatorText : ITranslator
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
    /// <summary>
    /// Translate given assembly into its equivalent string's bytes.
    /// </summary>
    /// <param name="target">An array containing the subsections of the instruction,</param>
    /// <returns>
    /// A byte array of length 2.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown if <c>parsedAssembly</c> does not have length equal to 1 or 3.</exception>
    public byte[] Translate(char[][] target)
    {
        return target.Length switch
        {
            1 => Encoding.UTF8.GetBytes(TranslateInstructionA(target[0])),
            3 => Encoding.UTF8.GetBytes(TranslateInstructionC(target[0], target[1], target[2])),
            _ => throw new ArgumentException($"Invalid parsing. Array of size {target.Length} is not a valid size.")
        };
    }
    
    private static string TranslateInstructionA(char[] addressString)
    {
        if (!int.TryParse(addressString, out int addressNumber))
            throw new TranslationException("Instruction A must contain a number");

        if (addressNumber is > 0x6000 or < 0)
            throw new ArgumentException($"Instruction A must contain a number from 0 to {0x6000}");
        
        return $"{addressNumber:b16}\n";
    }

    private string TranslateInstructionC(char[] computation, char[] destination, char[] jump)
    {
        var builder = new StringBuilder("111"); // start of C instruction binary
        builder.Append(TranslateComputation(new string(computation)));
        builder.Append(TranslateDestination(destination));
        builder.Append(TranslateJump(new string(jump)));
        builder.Append('\n');
        return builder.ToString();
    }

    private string TranslateComputation(string? computation)
    {
        computation ??= string.Empty;
        
        if (!_computationCodes.TryGetValue(computation, out var computationBinary))
            throw new TranslationException($"Unknown computation assembly: {computation}");

        return computationBinary;
    }
    
    private static string TranslateDestination(ReadOnlySpan<char> destination)
    {
        // checks if letter is at destination portion of the assembly and sets it's char in case it is
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
            throw new TranslationException($"Unknown jump assembly: {jump}");

        return jumpBinary;
    }
}