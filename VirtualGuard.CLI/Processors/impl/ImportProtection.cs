using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace VirtualGuard.CLI.Processors.impl;

public class ImportProtection : IProcessor
{
    public static readonly Dictionary<IMethodDescriptor, MethodDefinition> ProxyMethodCache =
        new Dictionary<IMethodDescriptor, MethodDefinition>();

    public static List<MethodDefinition> ScannedMethods = new List<MethodDefinition>();
    
    public string Identifier => "Import Protection";
    public void Process(Context ctx)
    {

        foreach (var type in ctx.Module.GetAllTypes())
        foreach (var method in type.Methods.Where(x =>
                     !ctx.Configuration.IsMemberExcluded(x, ctx) && !ctx.Configuration.IsMemberVirtualized(x, ctx) &&
                     !ctx.Virtualizer.IsMethodVirtualized(x)).ToArray())
        {

            if(method.CilMethodBody == null)
                continue;
            
            foreach (var instr in method.CilMethodBody.Instructions)
            {

                if (instr.Operand is not IMethodDescriptor desc)
                    continue;
                
                if(desc.Signature.HasThis) // impl later
                    continue;

                var proxy = CreateProxyMethod(desc, instr.OpCode.Code, ctx.Module.GetOrCreateModuleType());
                
                if(!ctx.Virtualizer.IsMethodVirtualized(proxy))
                    ctx.Virtualizer.AddMethod(proxy, true);

                instr.Operand = proxy;
            }
            
            ScannedMethods.Add(method);
        }
        
        
    }

    private static Random _rnd = new Random();
    
    MethodDefinition CreateProxyMethod(IMethodDescriptor desc, CilCode code, TypeDefinition parent)
    {

        if (ProxyMethodCache.TryGetValue(desc, out MethodDefinition foundDef))
            return foundDef;
        
        MethodDefinition def = new MethodDefinition(_rnd.Next().ToString("x"),
            MethodAttributes.Public | MethodAttributes.Static, desc.Signature);

        def.CilMethodBody = new CilMethodBody(def);

        var instrs = def.CilMethodBody.Instructions;

        foreach (var arg in def.Parameters)
        {
            instrs.Add(CilOpCodes.Ldarg, arg);
        }

        switch (code)
        {
            case CilCode.Call:
                instrs.Add(CilOpCodes.Call, desc);
                break;
            
            case CilCode.Callvirt:
                instrs.Add(CilOpCodes.Callvirt, desc);
                break;
            
            case CilCode.Newobj:
                instrs.Add(CilOpCodes.Newobj, desc);
                break;
        }

        instrs.Add(CilOpCodes.Ret);
        
        ProxyMethodCache.Add(desc, def);
        
        instrs.OptimizeMacros();
        
        parent.Methods.Add(def);

        return def;
    }
    
    
}