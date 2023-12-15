using AsmResolver.DotNet;
using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl;

public class VmCalls : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        foreach (var chunk in rt.VmChunks)
            foreach (var vmInstruction in chunk.Content.ToArray())
            {
                if (vmInstruction.OpCode != VmCode.Call)
                    continue;

                // resolve method def

                var method = vmInstruction.Operand as IMethodDescriptor;

                var def = method.Resolve();
                
                if(def == null)
                    continue;

                if (rt.IsMethodVirtualized(def, out VmChunk inlineTarget))
                {
                    chunk.Content.ReplaceRange(vmInstruction,
                        new VmInstruction(VmCode.Ldc_I4, def.Parameters.Count),
                                        new VmInstruction(VmCode.Ldc_I4, inlineTarget),
                                        new VmInstruction(VmCode.Vmcall, rt.Descriptor.Data.GetStartKey(inlineTarget))
                        );
                }

            }
    }
    
    
}