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
    
    public VmCode OpCode;
    public object Operand;
}