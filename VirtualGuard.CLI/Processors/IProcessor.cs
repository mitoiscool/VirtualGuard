namespace VirtualGuard.CLI.Processors;

public interface IProcessor
{
    public string Identifier { get; }
    public void Process(Context ctx);
}