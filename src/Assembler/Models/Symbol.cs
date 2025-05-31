namespace Assembler.Models;

public sealed record Symbol
{
    public required string Name { get; init; }
    public required short Value { get; init; }
}