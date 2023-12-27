using System.Diagnostics;
using System.Reflection.Emit;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using Echo.ControlFlow;

namespace VirtualGuard.AST;

public class AstBuilder
{
    public AstBuilder()
    {
        
    }

    private Stack<AstExpression> _evalStack = new Stack<AstExpression>();

    public void Reset()
    {
        _evalStack.Clear();
    }

    public AstBlock Analyze(ControlFlowNode<CilInstruction> node, bool returns)
    {
        var block = new AstBlock();

        foreach (var instr in node.Contents.Instructions)
        {
            
            // pop
            var pops = instr.GetStackPopCount(returns);
            
            if (instr.OpCode == CilOpCodes.Ret)
                pops = 0; // bad fix
            
            var popped = new AstExpression[pops == -1 ? 0 : pops]; // leave likes to use -1 - wtf bro
            
            for (int i = 0; i < pops; i++)
                popped[i] = _evalStack.Pop();
            
            // turn into expression
            var expr = AstExpression.Create(instr, popped);
            
            block.Add(expr);
            
            if(instr.OpCode == CilOpCodes.Dup) // repush original bc only peeked
                _evalStack.Push(popped[0]); // dup
            
            if(instr.GetStackPushCount() > 0)
                _evalStack.Push(expr);
            
            if(instr.OpCode == CilOpCodes.Leave)
                _evalStack.Clear(); // tbh if stack frame ends up not being cleared doesn't it mess it up anyways?

        }

        return block;
    }
    
    
    
}