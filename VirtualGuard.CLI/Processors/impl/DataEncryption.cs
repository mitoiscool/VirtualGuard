using System.Collections;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;

namespace VirtualGuard.CLI.Processors.impl;

public class DataEncryption : IProcessor
{
    public static Dictionary<object, MethodDefinition> _constantCache = new Dictionary<object, MethodDefinition>();
    public static List<MethodDefinition> ScannedMethods = new List<MethodDefinition>();

    public string Identifier => "DataEncryption";

    public void Process(Context ctx)
    {
        var rnd = new Random();
        
        foreach (var type in ctx.Module.GetAllTypes())
        foreach (var method in type.Methods.Where(x => !ctx.Configuration.IsMemberExcluded(x, ctx) && !ctx.Configuration.IsMemberVirtualized(x, ctx) && !ctx.Virtualizer.IsMethodVirtualized(x)).ToArray())
        {
            if(method.CilMethodBody == null || !method.CilMethodBody.Instructions.Any())
                continue;
            
            method.CilMethodBody.Instructions.ExpandMacros();
            var instrs = method.CilMethodBody.Instructions;
            
            foreach (var instruction in instrs.ToArray()) // keep being able to iterate through
            {
                if(instruction.OpCode.Code != CilCode.Ldstr && instruction.OpCode.Code != CilCode.Ldc_I4)
                    continue;
                
                if(!ScannedMethods.Contains(method))
                    ScannedMethods.Add(method);
                
                if(instruction.Operand == null)
                    continue;

                if (!_constantCache.TryGetValue(instruction.Operand, out var cachedMethod))
                { // if it can't get the cached value, create new one
                    

                    var newMethod = new MethodDefinition(rnd.Next().ToString("x"),
                        MethodAttributes.Static | MethodAttributes.Public,
                                new MethodSignature(
                                    CallingConventionAttributes.Default,
                                    instruction.OpCode.Code == CilCode.Ldc_I4 ? ctx.Module.CorLibTypeFactory.Int32 : ctx.Module.CorLibTypeFactory.String,
                                    Array.Empty<TypeSignature>())
                    );

                    newMethod.CilMethodBody = new CilMethodBody(newMethod);

                    var body = newMethod.CilMethodBody;

                    switch (Type.GetTypeCode(instruction.Operand.GetType()))
                    {
                        case TypeCode.String:
                            body.Instructions.Add(CilOpCodes.Ldstr, (string)instruction.Operand);
                            break;
                        
                        default:
                            body.Instructions.Add(CilOpCodes.Ldc_I4, (int)instruction.Operand);
                            break;
                    }
                    
                    body.Instructions.Add(CilOpCodes.Ret);
                    
                    ctx.Module.GetOrCreateModuleType().Methods.Add(newMethod);

                    cachedMethod = newMethod;
                    
                    _constantCache.Add(instruction.Operand, newMethod);
                    
                    // virtualize method
                    ctx.Virtualizer.AddMethod(cachedMethod, true);
                }
                
            }
            
            method.CilMethodBody.Instructions.OptimizeMacros();
        }
        
        // now remove cached tmp methods

        //foreach (var meth in _stringCache.Values)
        //{
        //    meth.DeclaringType.Methods.Remove(meth);
        //}
        
    }
    
}