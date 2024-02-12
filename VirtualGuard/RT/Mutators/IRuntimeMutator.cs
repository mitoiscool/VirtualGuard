using VirtualGuard.RT.Mutators.impl;
using VirtualGuard.RT.Mutators.impl.Runtime;

namespace VirtualGuard.RT.Mutators;

internal interface IRuntimeMutator
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
        
        new HandlerMutator(),
        
        new Renamer(),
        //new ControlFlow(),
        //new LocMutation(),
        //new HiddenFields(),

        // vmcode
        new TokenAllocator(),
        new EncodeStrings(),
        new BuildChunkKeys(),
        //new VirtualOpCodes(),
        new VmCalls(),
        new ChunkShuffler(),
        
        
        //new PseudoRegions(),
        //new ConstantMutation()
        
        new BranchMutator(),
        
        new FinalizeMutations()
    };

    public static IRuntimeMutator[] GetMutators() => _mutators;
}