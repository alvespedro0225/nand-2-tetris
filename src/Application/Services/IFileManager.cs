namespace Application.Services;

public interface IFileManager
{
    public StreamReader ReadFile(string source);
    public Task WriteToFileAsync(string source, byte[] content);
}