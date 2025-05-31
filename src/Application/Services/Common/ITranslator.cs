namespace Application.Services.Common;

public interface ITranslator
{
    public byte[] TranslateParsedAssembly(char[][] parsedAssembly);
}