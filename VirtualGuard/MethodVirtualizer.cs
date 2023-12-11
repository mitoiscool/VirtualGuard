using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;
using Echo.Platforms.AsmResolver;
using VirtualGuard.RT;
using VirtualGuard.RT.Chunk;
using VirtualGuard.Transform.IL;
using VirtualGuard.VMIL.Translation;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard;

public class MethodVirtualizer
{
    public MethodVirtualizer(VirtualGuardRT rt)
    {
        _rt = rt;
    }

    private VirtualGuardRT _rt;
    
    private bool _exported;
    private MethodDefinition _currentMethod;
    private ControlFlowGraph<CilInstruction> _cfg;
    private VmMethod _virtualizedMethod;

    public void Virtualize(MethodDefinition def, bool exported = false)
    {
        _exported = exported;
        _currentMethod = def;
        
        BuildCFG();
        
        TransformIL();
        
        BuildVMIL();
        
        TransformVMIL();
        
        BuildChunks();
        
        Finish();
    }

    private void BuildCFG()
    {
        _currentMethod.CilMethodBody.Instructions.ExpandMacros();
        _cfg = _currentMethod.CilMethodBody.ConstructStaticFlowGraph();
        
    }

    private void TransformIL()
    {
        foreach (var node in _cfg.Nodes)
        {
            foreach (var transformer in IILTransformer.GetTransformers())
            {
                transformer.Transform(node, _cfg);
            }
            
        }
        
        
    }
    
    private void BuildVMIL()
    {
        var newMethod = new VmMethod(_currentMethod, _rt);
        
        foreach (var node in _cfg.Nodes)
        {
            var block = newMethod.GetBlock(node); // providing node implicitly adds it to blockMap

            foreach (var instruction in node.Contents.Instructions)
                ITranslator.Lookup(instruction).Translate(instruction, block, newMethod);
        }

        
        _virtualizedMethod = newMethod;
    }

    private void TransformVMIL()
    {
        // maybe use abstraction or something here, but tbh not entirely sure what other transforms besides updating branches I'd need

        foreach (var instr in _virtualizedMethod.Content.SelectMany(x => x.Content))
        { // update branch locations
            if (instr.Operand is ControlFlowNode<CilInstruction> controlFlowNode)
                instr.Operand = _virtualizedMethod.GetTranslatedBlock(controlFlowNode);
        }
        
        
    }

    private void BuildChunks()
    {
        var blockChunkMap = new Dictionary<VmBlock, VmChunk>();

        foreach (var block in _virtualizedMethod.Content)
        {
            blockChunkMap.Add(block, block.BuildChunk(_virtualizedMethod, _rt));
        }
        
        // now run post code once all chunks built
        foreach (var block in _virtualizedMethod.Content)
        {
            block.OnChunksBuilt(blockChunkMap, _rt);
        }
        
    }

    private void Finish()
    {
        // deinit
        _currentMethod = null;
        _exported = false;
        _virtualizedMethod = null;
        _cfg = null;
    }
    
}