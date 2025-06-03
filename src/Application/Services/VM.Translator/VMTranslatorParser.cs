using Application.Services.Common;

namespace Application.Services.VM.Translator;

public sealed class VmTranslatorParser : IParser
{
    public char[][] Parse(ReadOnlySpan<char> target)
    {
        List<char[]> instructions = [];
        int last = 0;
        for (int i = 0; i < target.Length; i++)
        {
            if (target[i] == ' ')
            {
                instructions.Add(target[last..i].ToArray());
                last = i + 1;
            }
            
            if (i == target.Length - 1)
                instructions.Add(target[last..target.Length].ToArray());
        }

        return instructions.ToArray();
    }
}