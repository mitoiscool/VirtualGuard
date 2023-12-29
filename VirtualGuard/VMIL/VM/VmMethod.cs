using System.Data;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using VirtualGuard.AST;
using VirtualGuard.RT;
using VirtualGuard.VMIL.Translation;

namespace VirtualGuard.VMIL.VM;

public class VmMethod
{
    public VmMethod(MethodDefinition meth, VirtualGuardRT rt, bool export = true)
    {
        isExport = export;
        CilMethod = meth;
        Runtime = rt;

        if (isExport && CilMethod == null)
            throw new DataException("VmMethod is an export yet the provided method is null");
    }

    public bool isExport = false;
    public readonly List<VmBlock> Content = new List<VmBlock>();
    public VmBlock Entry => Content.First();
    public MethodDefinition CilMethod;
    public VirtualGuardRT Runtime;
    private VmVariable _tmpVar;

    private Dictionary<ControlFlowNode<CilInstruction>, VmBlock> _blockMap =
        new Dictionary<ControlFlowNode<CilInstruction>, VmBlock>();
    
    private readonly Dictionary<int, VmVariable> _argVariableMapping = new Dictionary<int, VmVariable>();
    private readonly Dictionary<int, VmVariable> _localVariableMapping = new Dictionary<int, VmVariable>();

    private List<VmVariable> _variables = new List<VmVariable>();
    
    private Dictionary<AstExpression, List<VmInstruction>> _translationMap =
        new Dictionary<AstExpression, List<VmInstruction>>();

    public void MarkTranslatedInstructions(VmInstruction[] instrs)
    {
        _translationMap.Last().Value.AddRange(instrs);
    }
    public ITranslator Begin(AstExpression expr)
    {
        _translationMap.Add(expr, new List<VmInstruction>());
        
        return ITranslator.Lookup(expr); // this is literally homemade reference proxy in my own code
    }

    public VmInstruction[] GetTranslatedInstructions(AstExpression expr)
    {
        return _translationMap[expr].ToArray();
    }

    public VmBlock GetTranslatedBlock(ControlFlowNode<CilInstruction> controlFlowNode) => _blockMap[controlFlowNode];
    public VmBlock GetBlock(ControlFlowNode<CilInstruction> originalNode)
    {
        var block = new VmBlock()
        {
            Parent = this
        };

        if (_blockMap.Count == 0)
        { // is first block, init args as locals

            var shuffledParams = CilMethod.Parameters.ToArray();
            shuffledParams.Shuffle();
            
            foreach (var parameter in shuffledParams)
            {
                var local = GetVariableFromArg(parameter.Index);

                block.WithArtificialContent(
                    new VmInstruction(VmCode.Dup), // dup onstack array of args
                    new VmInstruction(VmCode.Ldc_I4, parameter.Index), // load index onto stack
                    new VmInstruction(VmCode.Ldelem), // load index from arg array
                    new VmInstruction(VmCode.Stloc, local) // set value into local
                    );
            }

            block.WithArtificialContent(
                new VmInstruction(VmCode.Pop)
            ); // pop remaining arg object
            
            if (CilMethod.Signature.HasThis && CilMethod.Parameters.Count > 0 &&
                CilMethod.Parameters[0].ParameterType.FullName != CilMethod.DeclaringType.FullName)
            {
                // weird special case, param index in instrs is -1

                var loc = GetVariableFromArg(-1);
                
                // pop inst off stack and set to var

                block.WithArtificialContent(
                    new VmInstruction(VmCode.Stloc, loc)
                );

            }





        }
        
        Content.Add(block);
        _blockMap.Add(originalNode, block);

        return block;
    }

    public VmVariable GetTempVar()
    {
        if (_tmpVar == null)
            _tmpVar = GetVariable();

        return _tmpVar;
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