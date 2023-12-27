namespace VirtualGuard.RT.Dynamic;

public class MutationStep
{
    public MutationStep(int mod, Operation op)
    {
        Modifier = mod;
        Operation = op;
    }
    public int Modifier;
    public Operation Operation;
}