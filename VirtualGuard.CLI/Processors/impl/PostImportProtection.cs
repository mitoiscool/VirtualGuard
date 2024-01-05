using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;

namespace VirtualGuard.CLI.Processors.impl;

public class PostImportProtection : IProcessor
{
    public string Identifier => nameof(PostImportProtection);
    public void Process(Context ctx)
    {
        var proxies = ImportProtection.ProxyMethodCache;
        
        foreach (var scannedMethod in ImportProtection.ScannedMethods)
        {
            
            foreach (var instr in scannedMethod.CilMethodBody.Instructions.ToArray())
            {
                if(instr.Operand is not IMethodDescriptor desc)
                    continue;
                
                // ReSharper disable once SimplifyLinqExpressionUseAll
                if(!proxies.Any(x => x.Value.FullName == desc.FullName))
                    continue;
                
                // resolve proxy

                var proxy = proxies.Single(x => x.Value.FullName == desc.FullName).Value;
                
                if(proxy.Parameters.Count > 0)
                    continue;
                
                // now replace the ref bc it's easy

                scannedMethod.CilMethodBody.Instructions.ReplaceRange(instr, proxy.CilMethodBody.Instructions.SkipLast(1).ToArray());
                
                scannedMethod.CilMethodBody.Instructions.CalculateOffsets();
            }
            
            
        }

    }
}

/*

// get arg count
                var argCount = proxy.Parameters.Count;

                if (argCount > 0)
                {

                    // get index of call
                    var callIndex = scannedMethod.CilMethodBody.Instructions.IndexOf(instr);

                    // step back instructions to before the args are pushed to create object[]
                    var startIndex = callIndex - argCount;

                    var paramInstructions = new CilInstruction[argCount];

                    int index = 0;
                    for (int i = startIndex; i < callIndex; i++)
                        paramInstructions[index++] = scannedMethod.CilMethodBody.Instructions[i];

                    // now rebuild with object[]

                    // build instrs
                    List<CilInstruction> builtInstrs = new List<CilInstruction>();

                    var newStartInstr = new CilInstruction(CilOpCodes.Ldc_I4, argCount);
                    builtInstrs.Add(newStartInstr);
                    builtInstrs.Add(new CilInstruction(CilOpCodes.Newarr,
                        ctx.Module.CorLibTypeFactory.Object.ToTypeDefOrRef()));

                    index = 0;
                    foreach (var argInstr in paramInstructions)
                    {
                        builtInstrs.Add(new CilInstruction(CilOpCodes.Dup));
                        builtInstrs.Add(new CilInstruction(CilOpCodes.Ldc_I4, index));

                        builtInstrs.Add(new CilInstruction(argInstr.OpCode,
                            argInstr
                                .Operand)); // move here, this is probably bad cause it would mess up branching, we'd need to replace og instr w/ nop

                        builtInstrs.Add(new CilInstruction(CilOpCodes.Box,
                            proxy.Parameters[index].ParameterType.ToTypeDefOrRef()));

                        builtInstrs.Add(new CilInstruction(CilOpCodes.Stelem_Ref));

                        index++;
                    }



                    // replace branch targets to this instruction

                    foreach (var updateInstr in scannedMethod.CilMethodBody.Instructions.Where(x =>
                                 x.Operand is ICilLabel cl && paramInstructions.Any(x2 => x2.Offset == cl.Offset)))
                    {
                        // will not work on switches
                        updateInstr.Operand = newStartInstr.CreateLabel(); // fix ref
                    }

                    // remove original instructions
                    scannedMethod.CilMethodBody.Instructions.RemoveRange(paramInstructions);

                    // insert new instrs
                    scannedMethod.CilMethodBody.Instructions.InsertRange(startIndex, builtInstrs);

                }
                
                // after handling args replace call to reference vm entry used by the body
                // will use the correct call automatically based on args

                instr.Operand = proxy.CilMethodBody.Instructions.Single(x => x.Operand is IMethodDescriptor).Operand; // only one method ref here
                
                // update offsets
                scannedMethod.CilMethodBody.Instructions.CalculateOffsets();*/