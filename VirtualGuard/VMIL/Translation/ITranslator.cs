using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation;

internal interface ITranslator
{
    
    public void Translate(AstExpression instr, ControlFlowNode<CilInstruction> node, VmBlock block, VmMethod meth,
        VirtualGuardContext ctx);
    public bool Supports(AstExpression instr);


    private static readonly List<ITranslator> _translators;
    static ITranslator()
    {
        _translators = new List<ITranslator>();
        foreach (var type in typeof(ITranslator).Assembly.GetTypes()) {
            if (typeof(ITranslator).IsAssignableFrom(type) && !type.IsAbstract) {
                var handler = (ITranslator)Activator.CreateInstance(type);
                _translators.Add(handler);
            }
        }
    }

    public static ITranslator Lookup(AstExpression instr)
    {
        foreach (var translator in _translators)
            if (translator.Supports(instr))
                return translator;

        return null;
    }
    
}