using System.Data;
using System.Diagnostics;
using System.Text;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Chunk;

internal class VmChunk : IChunk
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

    public int GetInstructionOffset(VmInstruction target)
    {
        int offset = 0;
        
        foreach (var instr in Content)
        {
            if(instr == target)
                continue;

            offset += instr.GetSize();
        }

        return offset + Offset;
    }

    /*public void WriteBytes(BinaryWriter writer, VirtualGuardRT rt)
    {
        // encryption should be done here
        
        // get previous key

        byte key = rt.Descriptor.Data.GetStartKey(this);
        var handlerShifts = rt.Descriptor.Data.HandlerShifts;
        var operandShifts = rt.Descriptor.Data.ByteShifts;
        

        foreach (var instr in Content)
        {
            
            var handler = rt.Descriptor.OpCodes[instr.OpCode];

            //Console.WriteLine("dec: {0} enc: {1} key: {2}", handler, handler ^ key, key);
            

            // shift key for handler
            //key = (byte)(key + handler); // don't use custom rotating values yet
            
            // _key = (byte)((_key * Constants.HANDLER_ROT1) + dec + (Constants.HANDLER_ROT2 >> (Constants.HANDLER_ROT3 ^ Constants.HANDLER_ROT4)) * Constants.HANDLER_ROT5);
            
            
            

            // calculating initially will be difficult because everything depends on the key,
            // so we need to use set start keys for blocks to map things out instead
            // start keys will be passed into jmp instructions to help fix instructions
            // vmcalls will also require start keys but they will also need current keys passed in to return jmp
        }
        
    }*/
    
    public void WriteBytes(BinaryWriter writer, VirtualGuardRT rt)
    {
        var handlerShifts = rt.Descriptor.Data.HandlerShifts;
        var operandShifts = rt.Descriptor.Data.ByteShifts;

        byte key = rt.Descriptor.Data.GetStartKey(this);
        byte prevCode = 0;
        int index = 0;
        
        foreach (var instr in Content)
        {
            // to calculate fixups, we should find difference between new code and old, to inverse adding old to fixup

            var rawOpCode = rt.Descriptor.OpCodes[instr.OpCode];

            var fixupValue = (byte)(rawOpCode - prevCode);
            
            //Debug.Assert((byte)(prevCode + fixupValue) == rawOpCode);
            
            //Console.WriteLine("prev: {0} current: {1} diff: {2}", prevCode, rawOpCode, fixupValue);

            prevCode = rawOpCode; // mark previous

            // fixup value is essentially just the way to get to the current code from the previous code

            // always write fixup first because fixup is read last (hear me out lol)
            
            // mutate fixup value
            
            Console.WriteLine("writing instr " + instr.ToString());

            if(index > 0) // don't mutate if first instr
                fixupValue = rt.Descriptor.Data.EmulateFixupMutation(Content[index - 1].OpCode, fixupValue); // use last opcode

            writer.Write((byte)(fixupValue ^ key));
            
            key = (byte)((key * handlerShifts[0]) + fixupValue +
                         (handlerShifts[1] >> (handlerShifts[2] ^ handlerShifts[3])) * handlerShifts[4]);
            
            foreach (var b in GetOperandBytes(instr.Operand, rt))
            {
                writer.Write((byte)(b ^ key));

                key = (byte)((operandShifts[0] ^ operandShifts[1]) - (b + (operandShifts[2] * key)) ^ (operandShifts[3] + operandShifts[4]));
            }
            
            //Console.WriteLine("wrote fixup {0} then {1}", fixupValue, instr.Operand == null ? "" : instr.Operand);
            
            index++;
        }
    }
    
    byte[] GetOperandBytes(object operand, VirtualGuardRT rt)
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

        if (operand is TypeSignature sig)
            return BitConverter.GetBytes(sig.MakeStandAloneSignature().MetadataToken.ToInt32());

        if (operand is VmInstruction instr)
            return BitConverter.GetBytes(rt.VmChunks.Single(x => x.Content.Contains(instr)).GetInstructionOffset(instr)); // get instruction offset
        
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

        var oldContent = Content.ToArray();
        
        for (int i = splitIndex; i < this.Content.Count; i++)
        {
            newInstrs.Add(oldContent[i]); // add to new
            Content.Remove(oldContent[i]); // remove from this
        }

        var chunk = new VmChunk(newInstrs);
        rt.AddChunk(chunk, rt.IndexOfChunk(this) + 1);

        return chunk;
    }

    public override string ToString()
    {
        return "<chunk @ " + Offset + ">";
    }
}