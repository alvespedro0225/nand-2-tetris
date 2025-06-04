using System.Text;
using Core.Services.Common;

namespace Infrastructure.Files;

public sealed class FileService(string path) : IFileService
{
    public string FileName { get; private set; } = null!;
    private bool _isDirectory;
    private string _path = path;

    public IEnumerator<StreamReader> GetFiles()
    {
        _path = ReplaceTilde(_path);
        if (Path.EndsInDirectorySeparator(_path))
        {
            _isDirectory = true;
            foreach (var fileName in Directory.GetFiles(_path))
            {
                FileName = Path.GetFileNameWithoutExtension(fileName);
                var file = File.OpenText(fileName);
                yield return file;
            }
        }
        else
        {
            var fileName = ReplaceTilde(_path);
            var file = File.OpenText(fileName);
           yield return file;
        }
    }

    public async Task WriteToFileAsync(byte[] content, string? extension = null, string? destination = null) 
    {
        destination ??= _isDirectory ? _path + "out" : _path; 
        
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