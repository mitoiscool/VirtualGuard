using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Conv : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        var type = ctx.ResolveType(ctx.Stack.Pop().I4());
        
        ctx.Stack.Push(BaseVariant.Cast(ctx.Stack.Pop(), type));

        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}