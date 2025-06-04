namespace Assembler.Services;

public interface ITranslator
{
    public byte[] Translate(char[][] target);
}