namespace Core.Services.Common;

public interface IFileService
{
    public string FileName { get; }
    public StreamReader ReadFile();
    public Task WriteToFileAsync(byte[] content, string? extension = null);
}