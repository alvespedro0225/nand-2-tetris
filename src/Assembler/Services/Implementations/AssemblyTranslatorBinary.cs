using Core.Exceptions;

namespace Assembler.Services.Implementations;

// using ushort because Hack instructions are 16 bit long
public sealed class AssemblyTranslatorBinary : ITranslator
{
    private readonly Dictionary<string, ushort> _computationCodes = new()
    {
        { "0", 0b0101010 },
        { "1", 0b0111111 },
        { "D", 0b0001100 },
        { "A", 0b0110000 },
        { "M", 0b1110000 },
        { "-1", 0b0111010 },
        { "!D", 0b0001101 },
        { "!A", 0b0110001 },
        { "!M", 0b1110001 },
        { "-D", 0b0001111 },
        { "-A", 0b0110011 },
        { "-M", 0b1110011 },
        { "D+1", 0b0011111 },
        { "A+1", 0b0110111 },
        { "M+1", 0b1110111 },
        { "D-1", 0b0001110 },
        { "A-1", 0b0110010 },
        { "M-1", 0b1110010 },
        { "D+A", 0b0000010 },
        { "D+M", 0b1000010 },
        { "D-A", 0b0010011 },
        { "D-M", 0b1010011 },
        { "A-D", 0b0000111 },
        { "M-D", 0b1000111 },
        { "D&A", 0b0000000 },
        { "D&M", 0b1000000 },
        { "D|A", 0b0010101 },
        { "D|M", 0b1010101 }
    };

    private readonly Dictionary<string, ushort> _jumpCodes = new()
    {
        { string.Empty, 0b000 },
        { "JGT", 0b001 },
        { "JEQ", 0b010 },
        { "JGE", 0b011 },
        { "JLT", 0b100 },
        { "JNE", 0b101 },
        { "JLE", 0b110 },
        { "JMP", 0b111 }
    };

    /// <summary>
    /// Translate given assembly into its equivalent binary code.
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
            1 => TranslateInstructionA(target[0]),
            3 => TranslateInstructionC(target[0], target[1], target[2]),
            _ => throw new ArgumentException($"Invalid parsing. Array of size {target.Length} is not a valid size.")
        };
    }
    
    private static byte[] TranslateInstructionA(char[] addressString)
    {
        if (!ushort.TryParse(addressString, out var addressNumber))
            throw new TranslationException("Instruction A must contain a number");

        if (addressNumber > 0x6000)
            throw new ArgumentException($"Instruction A must contain a number from 0 to {0x6000}");

        var instructionBytes = BitConverter.GetBytes(addressNumber);
        return instructionBytes;
    }   

    private byte[] TranslateInstructionC(char[] computation, char[] destination, char[] jump)
    {
        ushort instructionBytes = 0b1110000000000000; // base C instruction has bits 0 - 2 set to 1
        var computationBits = TranslateComputation(computation);
        // sets instruction bits 3 - 9 to be equal to computation bytes without affecting the rest
        instructionBytes = (ushort) (computationBits | instructionBytes);
        var destinationBits = TranslateDestination(destination);
        // sets bits 10 - 12
        instructionBytes = (ushort) (destinationBits | instructionBytes);
        var jumpBits = TranslateJump(jump);
        // sets bits 13-15
        instructionBytes = (ushort)(jumpBits | instructionBytes); 
        return BitConverter.GetBytes(instructionBytes);
    }

    private ushort TranslateComputation(char[] computation)
    {
        if (!_computationCodes.TryGetValue(new string(computation), out var computationBits))
            throw new TranslationException($"Unknown computation assembly: {computation}");

        // shifts the bits to positions 3 - 9
        return (ushort)(computationBits << 6); 
    }

    private static ushort TranslateDestination(ReadOnlySpan<char> destination)
    {
        // checks if letter is at destination portion of the assembly and sets it's bit in case it is
        ushort destinationBits = 0;
        
        if (destination.ContainsAny('A', 'a'))
            destinationBits |= 1 << 2;

        if (destination.ContainsAny('D', 'd'))
            destinationBits |= 1 << 1;
        
        if (destination.ContainsAny('M', 'm'))
            destinationBits |= 1;

        // shifts bits to positions 10-12
        return (ushort)(destinationBits << 3);
    }

    private ushort TranslateJump(char[] jump)
    {
        if (!_jumpCodes.TryGetValue(new string(jump), out var jumpBits))
            throw new TranslationException($"Unknown jump assembly: {jump}");

        // no need shifting as they are the last ones
        return jumpBits;
    }

}