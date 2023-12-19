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

        if (!parent.Entry.Equals(this))
        { // not an entry chunk
            rt.AddChunk(chunk);
            return chunk; // not entry chunk
        }
        

        if (parent.isExport)
        {
            rt.AddExportChunk(chunk, parent.CilMethod);
        }
        else
        {
            rt.AddImportChunk(chunk, parent.CilMethod);
        }
        
        return chunk;
    }

    public void OnChunksBuilt(Dictionary<VmBlock, VmChunk> chunkMap, VirtualGuardRT rt)
    {
        // do initial updating of offsets

        var thisChunk = chunkMap[this];

        foreach (var content in thisChunk.Content)
        { // update jmps to use chunks; it's in ldc.i4 which is kinda ew edit: entertry contains block offset of handler
            
            if (content.OpCode != VmCode.Ldc_I4 || content.OpCode != VmCode.Entertry) // 
                continue;

            if(content.Operand is not VmBlock)
                continue;

            var oldBlock = (VmBlock)content.Operand;
            var equivalentChunk = chunkMap[oldBlock];

            content.Operand = equivalentChunk;
        }

    }
    
    
    
}