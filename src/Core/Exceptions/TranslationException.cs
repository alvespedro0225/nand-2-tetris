namespace Core.Exceptions;

public class TranslationException : Exception
{
    public TranslationException() {}
    public TranslationException(string message) : base(message) {}
}