namespace Core.Services.Common;

public interface IFileService
{
    public string FileName { get; }
    public IEnumerator<StreamReader> GetFiles();
    public Task WriteToFileAsync(byte[] content, string? destination, string? extension = null);
}