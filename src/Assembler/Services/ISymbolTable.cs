namespace Assembler.Services;

internal interface ISymbolTable
{
    public bool TryGetSymbol(string symbol, out int symbolValue);
    public bool TryAddSymbol(string symbol, int symbolValue);
    void AddSymbol(string symbol, int lineCounter);
}