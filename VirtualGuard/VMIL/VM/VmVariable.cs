namespace VirtualGuard.VMIL.VM;

internal class VmVariable
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