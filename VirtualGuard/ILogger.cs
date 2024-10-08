namespace VirtualGuard;

public interface ILogger
{
    public void Info(string msg);
    public void Success(string msg);
    public void Warning(string msg);
    public void Fatal(string msg);
}