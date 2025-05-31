namespace Assembler.Services.Implementations;

public sealed class Parser : IParser
{
    /// <summary>
    /// Parses valid Hack assembly code and returns the arrays corresponding to computation, destination and jump in
    /// case of a C instruction an array corresponding to address in case of an A instruction.
    /// </summary>
    /// <param name="assembly"> The assembly code to be parsed.</param>
    /// <returns></returns>
    public char[][] ParseAssembly(ReadOnlySpan<char> assembly)
    {
        return assembly[0] == '@' ? [assembly[1..].ToArray()] : ParseInstructionC(assembly);
    }
    
    private char[][] ParseInstructionC(ReadOnlySpan<char> assembly)
    {
        var computationStartIndex = 0; // assume dest is null
        var computationEndIndex = assembly.Length; // assumes jump is null
        
        if (assembly.Contains('='))
            computationStartIndex = assembly.IndexOf('=') + 1;
        
        if (assembly.Contains(';'))
            computationEndIndex = assembly.IndexOf(';');
        
        
        
        char[] destination = computationStartIndex == 0 ? [] : assembly[..(computationStartIndex - 1)].ToArray();
        char[] computation = assembly[computationStartIndex..computationEndIndex].ToArray();
        char[] jump = computationEndIndex == assembly.Length ? [] : assembly[(computationEndIndex + 1)..].ToArray();

        return [computation, destination, jump];
    }
}
