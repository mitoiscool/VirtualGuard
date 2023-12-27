namespace VirtualGuard.CLI.Processors.impl;

public class Virtualization : IProcessor
{
    public string Identifier => "Virtualization";

    public void Process(Context ctx)
    {
        // mark methods
        
        foreach (var virtualizedMethod in ctx.Configuration.ResolveVirtualizedMethods(ctx))
        {
            ctx.MarkForVirtualization(virtualizedMethod.Item1, virtualizedMethod.Item2);
        }
        
        ctx.CommitProcessors();
    }
    
}