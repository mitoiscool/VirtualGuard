using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Newarr : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {
        ctx.Stack.Push(new ArrayVariant(Array.CreateInstance(ctx.ResolveType(ctx.Reader.ReadInt().I4()), ctx.Stack.Pop().I4())));

        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}