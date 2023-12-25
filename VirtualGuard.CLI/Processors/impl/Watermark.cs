namespace VirtualGuard.CLI.Processors.impl;

public class Watermark : IProcessor
{
    public string Identifier => "Watermark";
    public void Process(Context ctx)
    {
        ctx.Module.Name = "virtualguard.io";
    }
}