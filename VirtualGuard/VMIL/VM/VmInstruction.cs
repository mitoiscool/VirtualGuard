using System.Data;
using AsmResolver.DotNet;
using VirtualGuard.RT.Chunk;

namespace VirtualGuard.VMIL.VM;

public class VmInstruction
{
    public VmInstruction(VmCode code)
    {
        OpCode = code;
    }

    public VmInstruction(VmCode code, object operand)
    {
        OpCode = code;
        Operand = operand;
    }

    public int GetSize()
    {
        if (this.Operand == null)
            return 1; // stop execution, length should not be updated and it should move to the next instr

        if (this.Operand is short)
            return sizeof(VmCode) + sizeof(short);
            
        if (this.Operand is int)
            return sizeof(VmCode) + sizeof(int);

        if (this.Operand is long)
            return sizeof(VmCode) + sizeof(long);

        if (this.Operand is VmVariable)
            return sizeof(VmCode) + sizeof(short);

        if (this.Operand is VmChunk)
            return sizeof(VmCode) + sizeof(int);

        if (this.Operand is IMetadataMember)
            return sizeof(VmCode) + sizeof(int);

        throw new DataException(this.Operand.GetType().FullName);
    }
    
    public VmCode OpCode;
    public object Operand;
}