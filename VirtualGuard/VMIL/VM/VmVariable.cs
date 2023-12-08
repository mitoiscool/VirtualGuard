namespace VirtualGuard.VMIL.VM;

public class VmVariable
{
    public VmVariable(short id)
    {
        Id = id;
    }
    
    public short Id;

    public override string ToString()
    {
        return "<Var: " + Id + ">";
    }
}