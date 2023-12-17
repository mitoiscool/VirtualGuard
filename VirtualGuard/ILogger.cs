namespace VirtualGuard;

public interface ILogger
{
    public void LogSuccess(string msg);
    public void LogWarning(string msg);
    public void LogFatal(string msg);
}