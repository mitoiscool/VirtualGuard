using System;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Ldloc : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            ctx.Stack.Push(ctx.Locals.GetLocal(ctx.Reader.ReadShort()));
            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;

    }
}