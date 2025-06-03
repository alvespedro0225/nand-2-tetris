namespace Application.Services.Common;

public interface ITranslator
{
    public byte[] Translate(char[][] target);
}