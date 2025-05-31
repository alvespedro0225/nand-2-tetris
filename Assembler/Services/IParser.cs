namespace Assembler.Services;

public interface IParser
{
    public char[][] ParseAssembly(ReadOnlySpan<char> assembly);
}