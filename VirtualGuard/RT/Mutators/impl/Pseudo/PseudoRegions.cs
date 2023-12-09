/*using VirtualGuard.RT.Chunk;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Mutators.impl.Pseudo;

public class PseudoRegions : IRuntimeMutator
{
    private Dictionary<VmCode, VmChunk> _pseudoInstructionChunkMapping = new Dictionary<VmCode, VmChunk>();
    private List<VmCode> _usedCodes = new List<VmCode>();
    void InitializeRegions()
    {
        
        // jmp (we don't need a return addr)

        var delemBuilder = new RegionBuilder(VmCode.__ldelem); // DEMO DEMO

        delemBuilder // return address on-stack
            .WithInstruction(VmCode.Ldc_I4, 10)
            .WithInstruction(VmCode.Ldc_I4, 10)
            .WithInstruction(VmCode.Add)
            .WithInstruction(VmCode.Jmp); // jmp to pushed return addr
        
        _pseudoInstructionChunkMapping.Add(delemBuilder.GetCode(), delemBuilder.GetChunk());

    }


    public void Mutate(VirtualGuardRT rt)
    {
        InitializeRegions();
        
        foreach (var vmChunk in rt.VmChunks.ToArray()) // toarray so we can add chunks
        {
            foreach (var instr in vmChunk.Content)
            {
                // just gonna test for ldelem
                if (instr.OpCode == VmCode.__ldelem)
                {
                    
                    if (!_usedCodes.Contains(VmCode.__ldelem))
                    {
                        rt.AddChunk(_pseudoInstructionChunkMapping[VmCode.__ldelem]);
                        _usedCodes.Add(VmCode.__ldelem);
                    }

                    instr.OpCode = VmCode.EnterRegion;
                    instr.Operand = _pseudoInstructionChunkMapping[VmCode.__ldelem];
                }
                
                
                
            }
            
            //rt.UpdateOffsets(); perf+
        }
        
        
    }
    
    
    
    
}*/