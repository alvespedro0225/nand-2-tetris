namespace Emulator;

public sealed class Hack
{
    // https://courses.cs.washington.edu/courses/cse390b/22wi/lectures/CSE390B-L08-hack-assembly_22wi.pdf
    private sealed class Memory
    {
        // Representation of the 16-bit registers from the Hack as a single 16-bit unit instead of 2 bytes
        private readonly byte[] _buffer = new byte[32];

        public short this[int index]
        {
            get
            {
                if (index < 0 || index >= _buffer.Length / 2)
                    throw new IndexOutOfRangeException();
                var bufferIndex = index * 2;
                return BitConverter.ToInt16(_buffer.AsSpan()[bufferIndex..(bufferIndex + 2)]);
            }
            set
            {
                ArgumentOutOfRangeException.ThrowIfNegative(value);
                if (index < 0 || index >= _buffer.Length / 2)
                    throw new IndexOutOfRangeException();
                
                var bufferIndex = index * 2;
                var valueBytes =  BitConverter.GetBytes(value);
                _buffer[bufferIndex] = valueBytes[0];
                _buffer[bufferIndex + 1] = valueBytes[1];
            }
        } 
    }
    private readonly Memory _memory = new Memory();
    private short _registerA;
    private short _registerD;
    public int Handle(byte[] buffer, int programCounter)
    {
        byte[] instructionBytes = [buffer[programCounter], buffer[programCounter + 1]];
        var instruction = BitConverter.ToInt16(instructionBytes, 0);
        var instructionA = instructionBytes[1] >> 7 == 0;
        var nextInstruction = programCounter + 2;
        var jump = false;
        
        if (instructionA)
        {
            _registerA = instruction;
            return programCounter + 2;
        }
        
        var aBitSet = (instructionBytes[0] >> 4 & 1) == 1; // shifts the A bit to the rightmost position and check if set
        var aluReferenceValue = aBitSet ? _registerA : _memory[_registerA];
        var aluOpInt = instruction >> 6; // shifts the alu operation bits to the rightmost position
        aluOpInt &= ~(1 << 6) ; // sets 7th bit to 0
        aluOpInt &= ~(1 << 7); // sets 8th bit to 0
        var aluOp = BitConverter.GetBytes(aluOpInt)[0]; // gets the last byte of the int
        short aluOut = aluOp switch
        {
            0b00101010 => 0,
            0b00111111 => 1,
            0b00111010 => -1,
            0b00001100 => _registerD,
            0b00110000 => aluReferenceValue,
            0b00001101 => (short) ~_registerD,
            0b00110001 => (short) ~aluReferenceValue, 
            0b00001111 => (short) -_registerD,
            0b00110011 => (short) -aluReferenceValue,
            0b00011111 => (short) (_registerD + 1),
            0b00110111 => (short) (_registerA + 1),
            0b00001110 => (short) (_registerD - 1),
            0b00110010 => (short) (aluReferenceValue - 1),
            0b00000010 => (short) (_registerD + aluReferenceValue),
            0b00010011 => (short) (_registerD - aluReferenceValue),
            0b00000111 => (short) (aluReferenceValue - _registerD),
            0b00000000 => (short) (_registerD & aluReferenceValue),
            0b00010101 => (short) (_registerD | aluReferenceValue),
            _ => throw new Exception("Invalid alu operation"),
        };

        if ((instruction >> 3 & 1) == 1)
            _memory[_registerA] = aluOut;
        
        if ((instruction >> 4 & 1) == 1)
            _registerD = aluOut;
        
        if ((instruction >> 5 & 1) == 1)
            _registerA = aluOut;
        
        // the three if's bellow check if the conditions for jumps are met by checking the jump bits
        // and alu output
        if ((instruction >> 2 & 1) == 1 && aluOpInt < 0)
            jump = true;
        
        if ((instruction >> 1 & 1) == 1 && aluOpInt == 0)
            jump = true;
        
        if ((instruction & 1) == 1 && aluOpInt > 0)
            jump = true;
        
        if (jump)
            nextInstruction = _registerA;
            
        return nextInstruction;
    }

    public void PrintResult()
    {
        Console.WriteLine($"Register 0: {_memory[0]}");
    }
}