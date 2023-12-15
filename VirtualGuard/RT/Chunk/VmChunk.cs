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
        
        // get previous key

        byte key = rt.Descriptor.Data.GetStartKey(this);
        var handlerShifts = rt.Descriptor.Data.HandlerShifts;
        

        foreach (var instr in Content)
        {
            
            var handler = rt.Descriptor.OpCodes[instr.OpCode];

            Console.WriteLine("dec: {0} enc: {1} key: {2}", handler, handler ^ key, key);
            writer.Write((byte)(handler ^ key));

            // shift key for handler
            //key = (byte)(key + handler); // don't use custom rotating values yet
            key = (byte)((key * handlerShifts[0]) + handler +
                         (handlerShifts[1] >> (handlerShifts[2] ^ handlerShifts[3])) * handlerShifts[4]);
            // _key = (byte)((_key * Constants.HANDLER_ROT1) + dec + (Constants.HANDLER_ROT2 >> (Constants.HANDLER_ROT3 ^ Constants.HANDLER_ROT4)) * Constants.HANDLER_ROT5);
            
            
            foreach (var b in GetOperandBytes(instr.Operand))
            {
                writer.Write((byte)(b ^ key));
                
                key = (byte)(key + b);
            }

            // calculating initially will be difficult because everything depends on the key,
            // so we need to use set start keys for blocks to map things out instead
            // start keys will be passed into jmp instructions to help fix instructions
            // vmcalls will also require start keys but they will also need current keys passed in to return jmp
            
        }
        
        
        
    }
    
    byte[] GetOperandBytes(object operand)
    {
        if (operand == null)
            return Array.Empty<byte>(); // write next instr

        if (operand is byte b)
            return new[] { b };
        
        if (operand is short s)
        {
            return BitConverter.GetBytes(s);
        }

        if (operand is int i)
        {
            return BitConverter.GetBytes(i);
        }

        if (operand is long l)
        {
            return BitConverter.GetBytes(l);
        }

        if (operand is VmVariable vv)
        {
            return BitConverter.GetBytes(vv.Id);
        }


        if (operand is VmChunk chunk)
        {
            return BitConverter.GetBytes(chunk.Offset); // trust this is updated
        }

        if (operand is IMetadataMember mem)
        {
            return BitConverter.GetBytes(mem.MetadataToken.ToInt32());
        }

        throw new DataException(operand.GetType().FullName);
    }

    int CalculateLength()
    {
        int length = 0;

        foreach (var instr in Content)
        {
            length += instr.GetSize();
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