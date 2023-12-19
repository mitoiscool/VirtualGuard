using VirtualGuard.RT.Mutators.impl;
using VirtualGuard.RT.Mutators.impl.Pseudo;

namespace VirtualGuard.RT.Mutators;

public interface IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx);

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
        
        // Runtime
        new InjectConstants(),
        new EncryptExceptions(),
        new Renamer(),

        // vmcode
        new EncodeStrings(),
        new BuildChunkKeys(),
        new VmCalls(),
        new ChunkShuffler(),
        
        
        //new PseudoRegions(),
        //new ConstantMutation()
        
        new BranchMutator(),
        
        new FinalizeMutations()
    };

    public static IRuntimeMutator[] GetMutators() => _mutators;
}