namespace Application.Services.Common;

public interface IParser
{
    public char[][] Parse(ReadOnlySpan<char> target);
}