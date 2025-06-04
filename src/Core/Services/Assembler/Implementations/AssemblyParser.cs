using Core.Services.Common;

namespace Core.Services.Assembler.Implementations;

public sealed class AssemblyParser : IParser
{
    /// <summary>
    /// Parses valid Hack assembly code and returns the arrays corresponding to its parts.
    /// </summary>
    /// <param name="target"> The assembly code to be parsed.</param>
    /// <returns>
    /// An array of either length 1 in case or an A instruction or 3 in case of a C instruction.
    /// For C instruction the order of the arrays is computation, destination, jump.
    /// </returns>
    public char[][] Parse(ReadOnlySpan<char> target)
    {
        return target[0] == '@' ? [target[1..].ToArray()] : ParseInstructionC(target);
    }

    /// <summary>
    /// Parses C instructions by finding the separators "=" and ";" and diving the source accordingly.
    /// </summary>
    /// <param name="assembly">The C instruction assembly</param>
    /// <returns>
    /// The arrays computation, destination and jump with their respective assembly.
    /// Destination and jump can be empty, computation can't.
    /// </returns>
    private static char[][] ParseInstructionC(ReadOnlySpan<char> assembly)
    {
        var computationStartIndex = 0; // assume dest is null
        var computationEndIndex = assembly.Length; // assumes jump is null

        if (assembly.Contains('='))
            computationStartIndex = assembly.IndexOf('=') + 1;

        if (assembly.Contains(';'))
            computationEndIndex = assembly.IndexOf(';');
        
        var destination = 
            computationStartIndex == 0 
                // sets to an empty array when destination is null. using computationStartIndex - 1 would raise an exception 
                ? [] 
                : assembly[..(computationStartIndex - 1)].ToArray(); 
        var computation = assembly[computationStartIndex..computationEndIndex].ToArray();
        var jump = 
            computationEndIndex == assembly.Length 
                // sets to an empty array when jump is null. using computationEndIndex + 1 would raise an exception 
                ? [] 
                : assembly[(computationEndIndex + 1)..].ToArray();

        return [computation, destination, jump];
    }
}