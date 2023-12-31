using System.Reflection.Metadata;
using AsmResolver.DotNet.Signatures;

namespace VirtualGuard.CLI.Processors.impl;

public class Renamer : IProcessor
{

    private Dictionary<MethodSignature, string> _sigMap = new Dictionary<MethodSignature, string>();
    
    public string Identifier => "Renamer";
    public void Process(Context ctx)
    {
        int typeIndex = 0;

        int abstractIndex = 0;

        foreach (var type in ctx.Module.GetAllTypes().Except(ctx.Virtualizer.GetVmTypes()))
        {
            int methodIndex = 0;

            if (ctx.Module.ManagedEntryPointMethod == null && type.IsPublic)
            { // idk how to unfuck this but it should work
                
            } else if (!ctx.Configuration.IsMemberExcluded(type, ctx))
            { // should rename
                type.Namespace = "";

                if (!type.IsModuleType && !type.IsRuntimeSpecialName)
                {
                    type.Name = "vg" + typeIndex++;
                }
                    
            }

            foreach (var method in type.Methods)
            {
                
                if(ctx.Module.ManagedEntryPointMethod == null && type.IsPublic && (method.IsPublic || type.IsInterface))
                    continue; // shouldn't rename
                
                if (!ctx.Configuration.IsMemberExcluded(method, ctx) && !method.IsRuntimeSpecialName)
                {
                    // handle abstract
                    
                    if(method.IsConstructor)
                        continue;
                    
                    if(method.IsAbstract || method.IsVirtual || (type.BaseType != null && type.BaseType.Resolve() != null && type.BaseType.Resolve().IsInterface && type.BaseType.Resolve().Methods.Select(x => x.Name).Contains(method.Name)))
                        continue; // fml

                    method.Name = "vg" + methodIndex++;
                }

                foreach (var arg in method.ParameterDefinitions)
                {
                    arg.Name = "vg";
                }
                
            }

            foreach (var field in type.Fields)
            {
                if(ctx.Module.ManagedEntryPointMethod == null && field.IsPublic && type.IsPublic)
                    continue; // shouldn't rename
                
                if (!ctx.Configuration.IsMemberExcluded(field, ctx) && !field.IsRuntimeSpecialName)
                    field.Name = "vg";
            }
            
        }
        
    }
}