using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using Echo;
using Echo.ControlFlow;
using Echo.ControlFlow.Regions;
using Echo.ControlFlow.Regions.Detection;
using Echo.Platforms.AsmResolver;
using VirtualGuard.Handlers;
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

    public void Virtualize(MethodDefinition def, bool exported = false)
    {
        _exported = exported;
        _currentMethod = def;
        
        MarkExceptionHandlers();
        
        BuildCfg();
        
        TransformIl();
        
        BuildVmil();
        
        TransformVmil();
        
        BuildChunks();
        
        Finish();
    }


    private void MarkExceptionHandlers()
    {
        /*var body = _currentMethod.CilMethodBody;
        
        if(body == null)
            _ctx.Logger.LogFatal(_currentMethod.FullName + " has no method body.");
        
        foreach (var exceptionHandler in body.ExceptionHandlers)
        {
            // add markers for try
            
            // resolve instructions for offsets
            var tryStartInstr = body.Instructions.Single(x => x.Offset == exceptionHandler.TryStart.Offset);

            var handlerStartInstr = body.Instructions.Single(x => x.Offset == exceptionHandler.HandlerStart.Offset);
            var handlerEndInstr = body.Instructions.Single(x => x.Offset == exceptionHandler.HandlerEnd.Offset);

            body.Instructions.ReplaceRange(tryStartInstr,
                new Marker(CilOpCodes.Nop, MarkerType.TryStart),
                tryStartInstr);

            body.Instructions.ReplaceRange(handlerStartInstr,
                new Marker(CilOpCodes.Nop, MarkerType.HandlerStart),
                handlerStartInstr);
            
            body.Instructions.ReplaceRange(handlerEndInstr,
                new Marker(CilOpCodes.Nop, MarkerType.HandlerEnd),
                handlerEndInstr);
        }*/

        
        
    }
    
    
    private void BuildCfg()
    {
        _currentMethod.CilMethodBody.Instructions.ExpandMacros();
        _cfg = _currentMethod.CilMethodBody.ConstructStaticFlowGraph();
        
        foreach (var region in _cfg.Regions)
        {
            if(region is not ExceptionHandlerRegion<CilInstruction> exceptionHandlerRegion)
                continue;
            
            exceptionHandlerRegion.ProtectedRegion.Entrypoint.Contents.Instructions.Insert(0, new Marker(CilOpCodes.Nop, MarkerType.TryStart));
            
            if(exceptionHandlerRegion.Handlers.Count > 1)
                _ctx.Logger.LogFatal("Virtualizer does not support multiple handlers on exception handler.");
            
            exceptionHandlerRegion.Handlers.First().Contents.Entrypoint.Contents.Instructions.Insert(0, new Marker(CilOpCodes.Nop, MarkerType.HandlerStart));
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
    
    private void BuildVmil()
    {
        var newMethod = new VmMethod(_currentMethod, _rt);
        
        foreach (var node in _cfg.Nodes)
        {
            var block = newMethod.GetBlock(node); // providing node implicitly adds it to blockMap

            foreach (var instruction in node.Contents.Instructions)
                ITranslator.Lookup(instruction).Translate(instruction, block, newMethod, _ctx);
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