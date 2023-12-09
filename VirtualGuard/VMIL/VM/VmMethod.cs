using System.Data;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;

namespace VirtualGuard.VMIL.VM;

public class VmMethod
{
    public VmMethod(MethodDefinition meth, bool export = true)
    {
        isExport = export;
        CilMethod = meth;

        if (isExport && CilMethod == null)
            throw new DataException("VmMethod is an export yet the provided method is null");
    }

    public bool isExport = false;
    public readonly List<VmBlock> Content = new List<VmBlock>();
    public VmBlock Entry => Content.First();
    public MethodDefinition CilMethod;

    private Dictionary<ControlFlowNode<CilInstruction>, VmBlock> _blockMap =
        new Dictionary<ControlFlowNode<CilInstruction>, VmBlock>();
    private readonly Dictionary<int, VmVariable> _argVariableMapping = new Dictionary<int, VmVariable>();
    private readonly Dictionary<int, VmVariable> _localVariableMapping = new Dictionary<int, VmVariable>();

    private List<VmVariable> _variables = new List<VmVariable>();

    public VmBlock GetTranslatedBlock(ControlFlowNode<CilInstruction> controlFlowNode) => _blockMap[controlFlowNode];
    public VmBlock GetBlock(ControlFlowNode<CilInstruction> originalNode)
    {
        var block = new VmBlock();

        if (_blockMap.Count == 0)
        { // is first block, init args as locals

            foreach (var parameter in CilMethod.Parameters)
            {
                var local = GetVariableFromArg(parameter.Index);

                block.WithContent(
                    new VmInstruction(VmCode.Dup), // dup onstack array of args
                    new VmInstruction(VmCode.Ldc_I4, parameter.Index), // load index onto stack
                    new VmInstruction(VmCode.Ldelem), // load index from arg array
                    new VmInstruction(VmCode.Stloc, local) // set value into local
                    );
            }

            block.WithContent(
                new VmInstruction(VmCode.Pop)
                ); // pop remaining arg object
            
            
        }
        
        Content.Add(block);
        _blockMap.Add(originalNode, block);

        return block;
    }
    
    
    VmVariable GetVariable()
    {
        var variable = new VmVariable((short)_variables.Count);
        _variables.Add(variable);

        return variable;
    }

    public VmVariable GetVariableFromLocal(int index)
    {
        if (_localVariableMapping.TryGetValue(index, out VmVariable var))
            return var;
        
        // doesn't exist
        var newVar = GetVariable();
        
        _localVariableMapping.Add(index, newVar);

        return newVar;
    }
    
    public VmVariable GetVariableFromArg(int index)
    {
        if (_argVariableMapping.TryGetValue(index, out VmVariable var))
            return var;
        
        // doesn't exist
        var newVar = GetVariable();
        
        _argVariableMapping.Add(index, newVar);

        return newVar;
    }
    
}