namespace VirtualGuard.CLI.Processors.impl;

public class Virtualization : IProcessor
{
    public string Identifier => "Virtualization";

    public void Process(Context ctx)
    {
        // mark methods

        if (ctx.License != LicenseType.Free)
        {
            foreach (var virtualizedMethod in ctx.Configuration.ResolveVirtualizedMethods(ctx))
            {
                ctx.Virtualizer.AddMethod(virtualizedMethod.Item1, !virtualizedMethod.Item2);
            }
        }

        // still commit because other methods could be virtualized
        ctx.Virtualizer.Commit();
    }
    
}