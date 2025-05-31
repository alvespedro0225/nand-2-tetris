namespace Application.Services.Common;

public interface IParser
{
    public char[][] ParseAssembly(ReadOnlySpan<char> assembly);
}