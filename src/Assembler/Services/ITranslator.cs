namespace Assembler.Services;

public interface ITranslator
{
    public string TranslateParsedAssembly(char[][] assembly);
}