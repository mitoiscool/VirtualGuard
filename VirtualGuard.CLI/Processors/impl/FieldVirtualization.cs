using AsmResolver.DotNet;

namespace VirtualGuard.CLI.Processors.impl;

public class FieldVirtualization : IProcessor
{
    public string Identifier => "Field Virtualization";
    public void Process(Context ctx)
    {
        foreach (var type in ctx.Module.GetAllTypes())
        foreach (var field in type.Fields)
        {
            
            
            
        }
            
    }

    /*TypeDefinition BuildHolderField()
    {
        
    }*/
}