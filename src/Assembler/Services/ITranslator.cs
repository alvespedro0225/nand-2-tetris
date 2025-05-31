namespace Assembler.Services;

public interface ITranslator
{
    public byte[] TranslateParsedAssembly(char[][] assembly);
}