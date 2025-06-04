using System.Text;
using Core.Services.Common;

namespace Infrastructure.Files;

public sealed class FileService(string path) : IFileService
{
    public string FileName { get; private set; } = null!;
    public StreamReader ReadFile()
    {
        FileName = Path.GetFileNameWithoutExtension(path);
        path = ReplaceTilde(path);
        var file = File.OpenText(path);
        return file;
    }

    public async Task WriteToFileAsync(byte[] content, string? extension = null)
    {
        var destination = ReplaceTilde(path);
        
        if (extension is not null)
            destination = Path.ChangeExtension(destination, extension);
        
        await using var file = File.OpenWrite(destination);
        await file.WriteAsync(content);
    }

    private static string ReplaceTilde(string path)
    {
        if (!path.StartsWith('~'))
            return path;
        
        var builder = new StringBuilder(path);
        var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        builder.Replace("~", userPath, 0, 1);
        return builder.ToString();
    }
}