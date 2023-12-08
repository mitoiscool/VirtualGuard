using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Stfld : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {

        var field = ctx.Stack.Pop().ToField();
        
        field.SetValue(ctx.Stack.Pop(), ctx.Stack.Pop());
        
        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}