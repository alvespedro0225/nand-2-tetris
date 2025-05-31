namespace Hack;

public static class Program
{
    public static void Main(string[] args)
    {
        // byte[] content = [
        //     0,
        //     2,
        //     236,
        //     16,
        //     0,
        //     3,
        //     224,
        //     144,
        //     0,
        //     0,
        //     227,
        //     8
        // ];
        // File.WriteAllBytes($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/code/cs/Hack/add", content);
        var buffer =
            File.ReadAllBytes($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/code/cs/Hack/add");
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