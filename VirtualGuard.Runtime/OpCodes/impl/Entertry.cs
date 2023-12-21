using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Regions;

namespace VirtualGuard.Runtime.OpCodes.impl;

public class Entertry : IOpCode
{
    public void Execute(VMContext ctx, out ExecutionState state)
    {

        var loc = ctx.Stack.Pop();
        var type = ctx.Stack.Pop();
        var entry = ctx.Reader.ReadByte();
        
        ctx.HandlerStack.Push(new ExceptionHandler(type.U1(), entry.U1(), loc.I4()));

        state = ExecutionState.Next;
    }

    public byte GetCode() => 0;
}