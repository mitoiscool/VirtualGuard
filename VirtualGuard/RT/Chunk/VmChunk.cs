using System.Data;
using System.Text;
using AsmResolver.DotNet;
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

    public void WriteBytes(BinaryWriter writer, VirtualGuardRT rt)
    {
        // encryption should be done here

        foreach (var instr in Content)
        {
            writer.Write(rt.Descriptor.OpCodes[instr.OpCode]);
            Console.WriteLine(instr.OpCode);
            
            if (instr.Operand == null)
                continue; // write next instr

            if (instr.Operand is short s)
            {
                writer.Write(s);
                continue;
            }

            if (instr.Operand is int i)
            {
                writer.Write(i);
                continue;
            }

            if (instr.Operand is long l)
            {
                writer.Write(l);
                continue;
            }


            if (instr.Operand is VmVariable vv)
            {
                writer.Write(vv.Id);
                continue;
            }


            if (instr.Operand is VmChunk chunk)
            {
                writer.Write(chunk.Offset); // trust this is updated
                continue;
            }

            if (instr.Operand is IMetadataMember mem)
            {
                writer.Write(mem.MetadataToken.ToInt32());
                Console.WriteLine(string.Join(", ", BitConverter.GetBytes(mem.MetadataToken.ToInt32())));
                continue;
            }
                

            throw new DataException(instr.Operand.GetType().FullName);
        }
        
        
        
    }

    int CalculateLength()
    {
        int length = 0;

        foreach (var instr in Content)
        {
            length += sizeof(VmCode);

            if (instr.Operand == null)
                continue; // stop execution, length should not be updated and it should move to the next instr

            if (instr.Operand is short)
                length += sizeof(short);
            
            if (instr.Operand is int)
                length += sizeof(int);

            if (instr.Operand is long)
                length += sizeof(long);

            if (instr.Operand is VmVariable)
                length += sizeof(short);

            if (instr.Operand is VmChunk)
                length += sizeof(int);

            if (instr.Operand is IMetadataMember)
                length += sizeof(int);

            // assume members and strings are already encoded as ints/strings
            // funny lol
        }

        return length;
    }

    public VmChunk Split(VirtualGuardRT rt, int splitIndex)
    {
        var newInstrs = new List<VmInstruction>();

        for (int i = splitIndex; i <= this.Content.Count; i++)
        {
            newInstrs.Add(Content[i]);
        }

        var chunk = new VmChunk(newInstrs);
        rt.AddChunk(chunk, rt.IndexOfChunk(this));

        return chunk;
    }

    public override string ToString()
    {
        return "<chunk @ " + Offset + ">";
    }
}