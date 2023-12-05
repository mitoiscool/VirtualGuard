using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Chunk;

public class VmChunk : IChunk
{
    public VmChunk(List<VmInstruction> instrs)
    {
        Content = instrs;
    }

    public List<VmInstruction> Content;

    public int Length => CalculateLength();
    public int Offset;
    
    public void OnOffsetComputed(int offset)
    {
        Offset = offset;
    }

    public void WriteBytes(BinaryWriter writer)
    {
        
    }

    int CalculateLength()
    {
        int length = 0;

        foreach (var instr in Content)
        {
            length += sizeof(VmCode);

            if (instr.Operand == null)
                continue; // stop execution, length should not be updated and it should move to the next instr

            if (instr.Operand is int)
                length += sizeof(int);

            if (instr.Operand is long)
                length += sizeof(long);

            if (instr.Operand is VmVariable)
                length += sizeof(short);
            
            // assume members and strings are already encoded as ints/strings
        }

        return length;
    }
    
    
    
    
}