using System.Text;
using Application.Services;
using Application.Services.Common;

namespace Infrastructure.Services;

public sealed class AssemblerFileManager : IFileManager
{
    private bool isLittleEndian = BitConverter.IsLittleEndian;
    public StreamReader ReadFile(string path)
    {
        path = ReplaceTilde(path);
        var file = File.OpenText(path);
        return file;
    }

    public async Task WriteToFileAsync(string source, byte[] content)
    {
        source = ReplaceTilde(source);
        var destination = Path.ChangeExtension(source, ".hack");
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