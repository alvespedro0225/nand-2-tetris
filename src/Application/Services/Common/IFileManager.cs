namespace Application.Services.Common;

public interface IFileManager
{
    public StreamReader ReadFile(string source);
    public Task WriteToFileAsync(string source, byte[] content);
}