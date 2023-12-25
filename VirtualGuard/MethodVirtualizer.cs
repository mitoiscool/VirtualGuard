using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using Echo;
using Echo.ControlFlow;
using Echo.ControlFlow.Regions;
using Echo.ControlFlow.Regions.Detection;
using Echo.Platforms.AsmResolver;
using VirtualGuard.AST;
using VirtualGuard.AST.IL;
using VirtualGuard.RT;
using VirtualGuard.RT.Chunk;
using VirtualGuard.Transform.IL;
using VirtualGuard.VMIL.Translation;
using VirtualGuard.VMIL.VM;

#pragma warning disable CS8602
namespace VirtualGuard;

public class MethodVirtualizer
{
    public MethodVirtualizer(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        _rt = rt;
        _ctx = ctx;
    }

    private VirtualGuardRT _rt;

    private VirtualGuardContext _ctx;
    private bool _exported;
    private MethodDefinition _currentMethod;
    private ControlFlowGraph<CilInstruction> _cfg;
    private VmMethod _virtualizedMethod;

    private Dictionary<ControlFlowNode<CilInstruction>, AstBlock> _astBlockMap = new Dictionary<ControlFlowNode<CilInstruction>, AstBlock>();

    public void Virtualize(MethodDefinition def, bool exported = false)
    {
        _exported = exported;
        _currentMethod = def;
        
        BuildCfg();
        
        TransformIl();
        
        BuildAst();
        
        BuildVmil();
        
        TransformVmil();
        
        BuildChunks();
        
        Finish();
    }
    
    
    private void BuildCfg()
    {
        _currentMethod.CilMethodBody.Instructions.ExpandMacros();
        _cfg = _currentMethod.CilMethodBody.ConstructStaticFlowGraph();
        
        foreach (var region in _cfg.Regions)
        {
            if(region is not ExceptionHandlerRegion<CilInstruction> exceptionHandlerRegion)
                continue;

            if(exceptionHandlerRegion.Handlers.Count > 1)
                _ctx.Logger.LogFatal("Virtualizer does not support multiple handlers on exception handler.");
            
            foreach (var handler in exceptionHandlerRegion.Handlers)
            {
                exceptionHandlerRegion.ProtectedRegion.Entrypoint.Contents.Instructions.Insert(0, new Marker(CilOpCodes.Nop, MarkerType.TryStart));

                // somehow get the exception handler type
                handler.Contents.Entrypoint.Contents.Instructions.Insert(0, new CilInstruction(CilOpCodes.Ldc_I4, _rt.Descriptor.ExceptionHandlers.GetFlag(((CilExceptionHandler)handler.Tag).HandlerType)));
            
                handler.Contents.Entrypoint.Contents.Instructions.Insert(1, new Marker(CilOpCodes.Nop, MarkerType.HandlerStart));
                
            }
            
            
        }
    }

    private void TransformIl()
    {
        foreach (var node in _cfg.Nodes)
        {
            foreach (var transformer in IILTransformer.GetTransformers())
            {
                transformer.Transform(node, _cfg);
            }
            
        }
        
    }

    private void BuildAst()
    {
        var builder = new AstBuilder();
        
        
        foreach (var cfgNode in _cfg.Nodes)
        {
            
            _astBlockMap.Add(cfgNode, builder.Analyze(cfgNode, _currentMethod.Signature.ReturnsValue));
            
        }
        
        builder.Reset(); // maybe doesn't work
        
    }
    
    private void BuildVmil()
    {
        var newMethod = new VmMethod(_currentMethod, _rt);
        
        foreach (var node in _cfg.Nodes)
        {
            var block = newMethod.GetBlock(node); // providing node implicitly adds it to blockMap

            foreach (var instruction in _astBlockMap[node]) // resolve ast block (this is kinda screwed)
                newMethod.Begin(instruction).Translate(instruction, block, newMethod, _ctx);
        }

        
        _virtualizedMethod = newMethod;
    }

    private void TransformVmil()
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