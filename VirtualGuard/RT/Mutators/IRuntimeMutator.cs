using VirtualGuard.RT.Mutators.impl;
using VirtualGuard.RT.Mutators.impl.Pseudo;

namespace VirtualGuard.RT.Mutators;

public interface IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt);

    static IRuntimeMutator()
    {
        /*var mutators = new List<IRuntimeMutator>();
        foreach (var type in typeof(IRuntimeMutator).Assembly.GetExportedTypes()) {
            if (typeof(IRuntimeMutator).IsAssignableFrom(type) && !type.IsAbstract) {
                var handler = (IRuntimeMutator)Activator.CreateInstance(type);
                mutators.Add(handler);
            }
        }

        _mutators = mutators.ToArray();*/
    }

    private static readonly IRuntimeMutator[] _mutators =
    { // need to do it manually to preserve order
        new InjectConstants(),
        new VmCalls(),
        //new PseudoRegions(),
        //new Renamer()
    };

    public static IRuntimeMutator[] GetMutators() => _mutators;
}