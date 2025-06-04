namespace Core.Services.Common;

public interface IFileManager
{
    public string FileName { get; }
    public StreamReader ReadFile(string source);
    public Task WriteToFileAsync(string source, byte[] content, string? extension = null);
}