namespace VirtualGuard.CLI.Processors.impl;

public class Virtualization : IProcessor
{
    public string Identifier => "Virtualization";
    public void Process(Context ctx)
    {
        
        // mark methods

        foreach (var virtualizedMethod in ctx.Configuration.ResolveVirtualizedMethods(ctx.Module))
        {
            ctx.Virtualizer.AddMethod(virtualizedMethod, true); // should probably setup exports
        }
        
        ctx.Virtualizer.CommitRuntime();
    }
    
}