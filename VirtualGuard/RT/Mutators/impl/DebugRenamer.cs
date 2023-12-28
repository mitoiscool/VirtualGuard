namespace VirtualGuard.RT.Mutators.impl;

public class DebugRenamer : IRuntimeMutator
{
    private static int Vm = 0; // scuffed but easy
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        if (!rt.isDebug)
            return;

        foreach (var type in rt.RuntimeModule.TopLevelTypes.Where(x => !x.IsModuleType && !x.IsRuntimeSpecialName))
        {
            type.Name = Vm + "_" + type.Name;
        }

        Vm++; // should only be called once per vm being committed
    }
}