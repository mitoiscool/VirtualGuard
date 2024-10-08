namespace VirtualGuard.CLI;

public class ConsoleLogger : ILogger
{
    public void Info(string msg)
    {
        Console.WriteLine("[i] " + msg);
    }

    public void Success(string msg)
    {
        Console.WriteLine("[+] " + msg);
    }

    public void Warning(string msg)
    {
        Console.WriteLine("[!] " + msg);
    }

    public void Fatal(string msg)
    {
        Console.WriteLine("[FATAL] " + msg);
        Environment.Exit(-1);
    }

    public void Verbose(string msg)
    {
        Console.WriteLine("[v] " + msg);
    }
}