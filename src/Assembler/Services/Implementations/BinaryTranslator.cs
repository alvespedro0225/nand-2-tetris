namespace Assembler.Services.Implementations;

public sealed class BinaryTranslator : ITranslator
{
    private bool _littleEndian;
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
    public BinaryTranslator()
    {
        _littleEndian = BitConverter.IsLittleEndian;
    }

    public byte[] TranslateParsedAssembly(char[][] assembly)
    {
        return assembly.Length switch
        {
            1 => TranslateInstructionA(assembly[0]),
            3 => TranslateInstructionC(assembly[0], assembly[1], assembly[2]),
            _ => throw new Exception("Invalid parsing")
        };
    }
    
    private byte[] TranslateInstructionA(char[] assembly)
    {
        if (!short.TryParse(assembly, out var addressNumber)) // using short because instructions are 16 bit
            throw new Exception("Invalid instruction");

        var instructionBytes = BitConverter.GetBytes(addressNumber);
        instructionBytes = _littleEndian ? instructionBytes : instructionBytes.Reverse().ToArray();
        return instructionBytes;
    }   

    private byte[] TranslateInstructionC(char[] computation, char[] destination, char[] jump)
    {
        ushort instructionBytes = 0b1110000000000000; // base C instruction has bits 0 - 2 set to 1
        // sets instruction bits 3 - 9 to be equal to computation bytes without affecting the rest
        var computationBits = TranslateComputation(computation);
        instructionBytes = (ushort) (computationBits | instructionBytes);
        // sets bits 10 - 12
        var destinationBits = TranslateDestination(destination);
        instructionBytes = (ushort) (destinationBits | instructionBytes);
        // sets bits 13-15
        var jumpBits = TranslateJump(jump);
        instructionBytes = (ushort)(jumpBits | instructionBytes); 
        return BitConverter.GetBytes(instructionBytes);
    }

    private ushort TranslateComputation(char[] computation)
    {
        if (!_computationCodes.TryGetValue(new string(computation), out var computationBits))
            throw new Exception("Invalid computation");

        // shifts the bits to positions 3 - 9
        return (ushort)(computationBits << 6); 
    }

    private ushort TranslateDestination(ReadOnlySpan<char> destination)
    {
        ushort destinationBits = 0;
        if (destination.ContainsAny('A', 'a'))
            destinationBits = (ushort)(destinationBits | (1 << 2));

        if (destination.ContainsAny('D', 'd'))
            destinationBits = (ushort)(destinationBits | (1 << 1));
        
        if (destination.ContainsAny('M', 'm'))
            destinationBits = (ushort)(destinationBits | (1 << 0));

        // shifts bits to positions 10-12
        return (ushort)(destinationBits << 3);
    }

    private ushort TranslateJump(char[] jump)
    {
        if (!_jumpCodes.TryGetValue(new string(jump), out var jumpBits))
            throw new Exception("Invalid jump assembly");

        // no need shifting as they are the last ones
        return jumpBits;
    }

}