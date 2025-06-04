using Core.Services.Common;
using Cocona;
using VmTranslator.Services;

namespace VmTranslator.Commands;

public static class VmTranslator
{
    public static void AddVmTranslatorCommands(this CoconaApp app)
    {
        app.AddCommand(TranslateVmCode);
    }

    private static async Task TranslateVmCode(
        IParser parser,
        ITranslator translator,
        IFileService fileService)
    {
        using var file = fileService.ReadFile();

        List<byte> instructions = [];
        
        while (!file.EndOfStream)
        {
            var line = await file.ReadLineAsync();
            
            if (string.IsNullOrEmpty(line) || line.StartsWith('/'))
                continue;

            var parsedLine = parser.Parse(line);

            instructions.AddRange(translator.Translate(parsedLine, fileService.FileName));
        }

        await fileService.WriteToFileAsync(instructions.ToArray(), ".asm");
    }
}