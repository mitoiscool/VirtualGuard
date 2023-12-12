using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Jnz : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        var loc = ctx.Stack.Pop();
        var flag = ctx.Stack.Pop().I2();
            
        if (flag != 0)
        {
            ctx.Reader.SetValue(loc.I4());
        }

        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}