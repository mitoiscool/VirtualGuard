using System.Diagnostics;
using VirtualGuard.RT;
using VirtualGuard.RT.Chunk;

namespace VirtualGuard.VMIL.VM;

public class VmBlock
{
    public List<VmInstruction> Content = new List<VmInstruction>();

    public VmBlock WithContent(params VmInstruction[] instrs)
    {
        Content.AddRange(instrs);
        return this;
    }
    
    public VmChunk BuildChunk(VmMethod parent, VirtualGuardRT rt)
    {
        // do some transforms, for ex remove the idea of the locals using the ctx

        var chunk = new VmChunk(this.Content);

        if (parent.isExport && parent.Entry.Equals(this))
        { // export chunk
            rt.AddExportChunk(chunk, parent.CilMethod);
        }
        else
        {
            rt.AddChunk(chunk);
        }
        
        return chunk;
    }

    public void OnChunksBuilt(Dictionary<VmBlock, VmChunk> chunkMap, VirtualGuardRT rt)
    {
        // do initial updating of offsets

        var thisChunk = chunkMap[this];

        foreach (var content in thisChunk.Content)
        { // update jmps to use chunks; keep in mind on encoding time we should change __jmploc to ldc_i4 and get the latest offset
            
            if (content.OpCode != VmCode.__jmploc)
                continue;

            Debug.Assert(content.Operand.GetType() == typeof(VmBlock)); // should be a vmblock at this point

            var oldBlock = (VmBlock)content.Operand;
            var equivalentChunk = chunkMap[oldBlock];

            content.Operand = equivalentChunk;
        }

    }
    
    
    
}