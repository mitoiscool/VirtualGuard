using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Leave : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        // we can check if there is a finally here and jmp

        ctx.CatchStack.Pop(); // pop off stack
        
        var key = ctx.Reader.ReadByte();
            
        ctx.Reader.SetKey(key.U1());
        ctx.Reader.SetValue(ctx.Stack.Pop().I4());
        
        // reset stack
        
        ctx.Stack.SetValue(0);

        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}