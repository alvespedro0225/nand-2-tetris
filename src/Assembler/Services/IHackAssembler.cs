namespace Assembler.Services;

public interface IHackAssembler
{
    public Task Assemble(string source);
}