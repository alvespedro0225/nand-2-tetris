using System.Text;

namespace Emulator;

public static class Program
{
    public static void Main(string[] args)
    {
        var buffer = 
            File.ReadAllBytes($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/code/cs/nand2Tetris/tests/Assembler/Add.hack");
        var hack = new Hack();
        var size = buffer.Length;
        var pc = 0;
        while (pc < size)
        {
            pc = hack.Handle(buffer, pc);
        }
        
        hack.PrintResult();
    }
}