namespace VmTranslator.Services;

public interface ITranslator
{
    public byte[] Translate(char[][] target, string fileName);
}