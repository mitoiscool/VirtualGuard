using AsmResolver.PE.DotNet.Cil;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.VMIL.Translation;

public interface ITranslator
{
    
    public void Translate(CilInstruction instr, VmBlock block, VmMethod meth);
    public bool Supports(CilInstruction instr);


    private static readonly List<ITranslator> _translators;
    static ITranslator()
    {
        _translators = new List<ITranslator>();
        foreach (var type in typeof(ITranslator).Assembly.GetExportedTypes()) {
            if (typeof(ITranslator).IsAssignableFrom(type) && !type.IsAbstract) {
                var handler = (ITranslator)Activator.CreateInstance(type);
                _translators.Add(handler);
            }
        }
    }

    public static ITranslator Lookup(CilInstruction instr)
    {
        foreach (var translator in _translators)
            if (translator.Supports(instr))
                return translator;

        throw new NotSupportedException(instr.ToString());
    }
    
}