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
    private Dictionary<string, MethodDefinition> _stringCache = new Dictionary<string, MethodDefinition>();

    public string Identifier => "DataEncryption";

    public void Process(Context ctx)
    {
        if(!ctx.Configuration.UseDataEncryption)
            return;
        
        var rnd = new Random();
        
        foreach (var type in ctx.Module.GetAllTypes())
        foreach (var method in type.Methods.Where(x => !ctx.Configuration.IsMemberExcluded(x, ctx)).ToArray())
        {
            if(method.CilMethodBody == null || !method.CilMethodBody.Instructions.Any())
                continue;

            var instrs = method.CilMethodBody.Instructions;
            
            foreach (var instruction in instrs.ToArray()) // keep being able to iterate through
            {
                
                if(instruction.OpCode.Code != CilCode.Ldstr)
                    continue;
                
                if(instruction.Operand == null)
                    continue;

                if (!_stringCache.TryGetValue((string)instruction.Operand, out var cachedMethod))
                { // if it can't get the cached value, create new one
                    

                    var newMethod = new MethodDefinition(rnd.Next().ToString("x"),
                        MethodAttributes.Static | MethodAttributes.Public,
                                new MethodSignature(
                                    CallingConventionAttributes.Default,
                                    ctx.Module.CorLibTypeFactory.String,
                                    Array.Empty<TypeSignature>())
                    );

                    newMethod.CilMethodBody = new CilMethodBody(newMethod);

                    var body = newMethod.CilMethodBody;

                    body.Instructions.Add(CilOpCodes.Ldstr, (string)instruction.Operand);
                    body.Instructions.Add(CilOpCodes.Ret);
                    
                    type.Methods.Add(newMethod);

                    cachedMethod = newMethod;
                    
                    _stringCache.Add((string)instruction.Operand, newMethod);
                    
                    // virtualize method
                    ctx.Virtualizer.AddMethod(newMethod, true);
                }
                
                // now replace ref
                
                instruction.ReplaceWith(CilOpCodes.Call, cachedMethod);
            }
            
        }
        
        
        
    }
    
    
}