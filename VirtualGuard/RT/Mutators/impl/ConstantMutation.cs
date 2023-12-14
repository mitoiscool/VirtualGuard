using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl;

public class ConstantMutation : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        Random rnd = new Random();
        
        foreach (var vmChunk in rt.VmChunks)
        {

            foreach (var vmInstruction in vmChunk.Content.ToArray())
            {
                
                if(vmInstruction.OpCode != VmCode.Ldc_I4)
                    continue;
                
                // do different behaviors for the significance

                if (vmInstruction.Operand is int i && rnd.Next(4) == 0) // mutate 1/5
                {
                    var mutatorKey = rnd.Next(100000000);

                    var mutatedValue = i - mutatorKey;
                    
                    vmChunk.Content.ReplaceRange(
                        vmInstruction, // existing item

                        new VmInstruction(VmCode.Ldc_I4, mutatedValue), // override original value
                        
                        new VmInstruction(VmCode.Ldc_I4, mutatorKey),
                        
                        new VmInstruction(VmCode.Add)
                    );
                    
                }

                if (vmInstruction.Operand is VmChunk chunk)
                {
                    // alloc instructions
                    var mutationKey = rnd.Next(1000000);
                    
                    vmChunk.Content.ReplaceRange(
                        vmInstruction,
                        
                        new VmInstruction(VmCode.Ldc_I4, chunk.Offset + 6 + mutationKey), // 5 for ldc.i4 1 for sub
                        new VmInstruction(VmCode.Ldc_I4, mutationKey),
                        new VmInstruction(VmCode.Sub)
                    );
                    
                    rt.UpdateOffsets();
                    
                }
                
            }
            
            
        }
        
        
        
    }
}