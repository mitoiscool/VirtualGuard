using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Regions;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Entertry : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {

        var loc = ctx.Stack.Pop();
        var entry = ctx.Reader.ReadByte();
        
        ctx.CatchStack.Push(new CatchRegion(loc.I4(), entry.U1()));

        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}