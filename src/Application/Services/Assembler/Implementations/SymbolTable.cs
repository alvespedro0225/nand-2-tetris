namespace Application.Services.Assembler.Implementations;

public sealed class SymbolTable : ISymbolTable
{
    private readonly Dictionary<string, int> _symbolTable = new()
    {
        { "SP", 0 },
        { "LCL", 1 },
        { "ARG", 2 },
        { "THIS", 3 },
        { "THAT", 4 },
        { "SCREEN", 0x4000 },
        { "KBD", 0x6000 },
        { "R0", 0 },
        { "R1", 1 },
        { "R2", 2 },
        { "R3", 3 },
        { "R4", 4 },
        { "R5", 5 },
        { "R6", 6 },
        { "R7", 7 },
        { "R8", 8 },
        { "R9", 9 },
        { "R10", 10 },
        { "R11", 11 },
        { "R12", 12 },
        { "R13", 13 },
        { "R14", 14 },
        { "R15", 15 }
    };
    public bool TryGetSymbol(string symbol, out int symbolValue)
    {
        return _symbolTable.TryGetValue(symbol, out symbolValue);
    }

    public bool TryAddSymbol(string symbol, int symbolValue)
    {
        return _symbolTable.TryAdd(symbol, symbolValue);
    }

    public void AddSymbol(string symbol, int lineCounter)
    {
        _symbolTable.Add(symbol, lineCounter);
    }
}