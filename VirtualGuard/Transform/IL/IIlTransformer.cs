using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.Transform.IL.impl;

namespace VirtualGuard.Transform.IL;

public interface IILTransformer
{
    public void Transform(ControlFlowNode<CilInstruction> node, ControlFlowGraph<CilInstruction> ctx);

    private static readonly IILTransformer[] _transformers =
    {
        new BlockConnector(),
        new BranchTargetUpdater(),
        new Nullifier() // needs to be last to not break branches targetting nop instrs
    };
    
    /*static IILTransformer()
    {
        var visitors = new List<IILTransformer>();
        foreach (var type in typeof(IILTransformer).Assembly.GetExportedTypes()) {
            if (typeof(IILTransformer).IsAssignableFrom(type) && !type.IsAbstract) {
                var handler = (IILTransformer)Activator.CreateInstance(type);
                visitors.Add(handler);
            }
        }

        _transformers = visitors.ToArray();
    }*/

    public static IILTransformer[] GetTransformers() => _transformers;
}