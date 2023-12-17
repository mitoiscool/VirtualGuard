namespace VirtualGuard.CLI;

public class ConsoleLogger : ILogger
{
    public void LogSuccess(string msg)
    {
        Console.WriteLine("[+] " + msg);
    }

    public void LogWarning(string msg)
    {
        Console.WriteLine("[!] " + msg);
    }

    public void LogFatal(string msg)
    {
        Console.WriteLine("[FATAL] " + msg);
        Environment.Exit(-1);
    }
}