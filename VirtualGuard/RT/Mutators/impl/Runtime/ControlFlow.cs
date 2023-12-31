using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.ReadyToRun;
using Echo.ControlFlow;
using Echo.Platforms.AsmResolver;
using VirtualGuard.RT.Dynamic;

namespace VirtualGuard.RT.Mutators.impl.Runtime;

internal class ControlFlow : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        // use echo to make graph

        var rnd = new Random();

        foreach (var method in rt.RuntimeModule.GetAllTypes().SelectMany(x => x.Methods.Where(x => x.CilMethodBody != null && x.CilMethodBody.Instructions.Count > 3)))
        {
            int endKey = rnd.Next();
            
            method.CilMethodBody.Instructions.ExpandMacros();
            var graph = method.CilMethodBody.ConstructStaticFlowGraph();

            if(graph.Nodes.Count == 1) continue;
            if(graph.Nodes.Count > 2) continue;

            var mapping = new Dictionary<ControlFlowNode<CilInstruction>, int>();

            foreach (var node in graph.Nodes)
                mapping.Add(node, rnd.Next());

            method.CilMethodBody.Instructions.Clear();
            var instrs = method.CilMethodBody.Instructions;

            var retHolder = new CilLocalVariable(method.Signature.ReturnType);
            
            var ctxLoc = new CilLocalVariable(rt.RuntimeModule.CorLibTypeFactory.UInt32);
            method.CilMethodBody.LocalVariables.Add(ctxLoc);
            method.CilMethodBody.LocalVariables.Add(retHolder);

            instrs.Add(CilOpCodes.Ldc_I4, mapping[graph.Entrypoint]);
            instrs.Add(CilOpCodes.Stloc, ctxLoc);

            instrs.Add(CilOpCodes.Ldnull);
            instrs.Add(CilOpCodes.Stloc, retHolder);

            // entry
            instrs.Add(CilOpCodes.Nop);
            var dispatchStart = instrs.Last().CreateLabel();

            foreach (var node in graph.Nodes)
            {
                // see if ctx is node id, 

                var endInstr = new CilInstruction(CilOpCodes.Nop);
                

                instrs.AddRange(new []
                {
                    new CilInstruction(CilOpCodes.Ldloc, ctxLoc),
                    new CilInstruction(CilOpCodes.Ldc_I4, mapping[node]),
                    new CilInstruction(CilOpCodes.Ceq),
                    new CilInstruction(CilOpCodes.Brfalse, endInstr.CreateLabel())
                });

                // now add all the node's instructions
                
                instrs.AddRange(node.Contents.Instructions);

                // now update context
                if (instrs.Last().OpCode == CilOpCodes.Br)
                {
                    instrs.Last().ReplaceWith(CilOpCodes.Ldc_I4, mapping[node.UnconditionalEdge.Target]);
                }
                else
                {
                    if (node.UnconditionalEdge != null)
                    {
                        instrs.Add(CilOpCodes.Ldc_I4, mapping[node.UnconditionalEdge.Target]);
                    } 
                    else if(node.Contents.Footer.OpCode != CilOpCodes.Ret)
                        instrs.Add(CilOpCodes.Ldc_I4, mapping[node]);
                    
                    if(node.Contents.Footer.OpCode == CilOpCodes.Ret)
                    {
                        if (!method.Signature.ReturnsValue)
                            instrs.Add(CilOpCodes.Ldnull);
                        instrs[method.Signature.ReturnsValue ? instrs.Count - 1 : instrs.Count - 2].ReplaceWith(CilOpCodes.Stloc, retHolder);
                        instrs.Add(CilOpCodes.Ldc_I4, endKey);
                    }
                    
                }

                instrs.Add(CilOpCodes.Stloc, ctxLoc);
                
                instrs.Add(endInstr);
            }

            instrs.Add(CilOpCodes.Ldloc, ctxLoc);
            instrs.Add(CilOpCodes.Ldc_I4, endKey);
            instrs.Add(CilOpCodes.Ceq);
            instrs.Add(CilOpCodes.Brfalse, dispatchStart);

            instrs.Add(CilOpCodes.Ldloc, retHolder);
            instrs.Add(CilOpCodes.Ret);
            
            // finds a way to return it seems like
            
            method.CilMethodBody.ComputeMaxStack();
            method.CilMethodBody.Instructions.OptimizeMacros();
        }
        
        
        
    }
}