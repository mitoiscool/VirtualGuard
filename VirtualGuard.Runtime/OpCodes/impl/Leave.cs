using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Leave : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        // we can check if there is a finally here and jmp
        
        var previousTry = ctx.HandlerStack.Pop(); // pop off stack
        
        var pos = ctx.Stack.Pop();
        var key = ctx.Reader.ReadByte();
        
        ctx.Stack.SetValue(0); // reset stack

        if (previousTry.Type == Constants.FinallyFL)
        { // do finally
            
            ctx.Reader.SetKey(previousTry.HandlerStartKey);
            ctx.Reader.SetValue(previousTry.HandlerPos);
            
        }
        else
        {
            ctx.Reader.SetKey(key.U1());
            ctx.Reader.SetValue(pos.I4()); // is this correct?
        }
        

        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}