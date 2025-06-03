using Application.Services.Common;
using Cocona;

namespace VmTranslator.Commands;

public static class VmTranslator
{
    private static string _source = null!;
    
    public static void AddVmTranslatorCommands(this CoconaApp app, string source)
    {
        _source = source;   
        app.AddCommand(TranslateVmCode);
    }

    private static async Task TranslateVmCode(
        IParser parser,
        ITranslator translator,
        IFileManager fileManager)
    {
        using var file = fileManager.ReadFile(_source);

        List<byte> instructions = [];
        
        while (!file.EndOfStream)
        {
            var line = await file.ReadLineAsync();
            
            if (string.IsNullOrEmpty(line) || line.StartsWith('/'))
                continue;

            var parsedLine = parser.Parse(line);

            instructions.AddRange(translator.Translate(parsedLine));
        }

        await fileManager.WriteToFileAsync(_source, instructions.ToArray(), ".asm");
    }
}