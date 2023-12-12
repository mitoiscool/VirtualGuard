using System;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Stloc : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            ctx.Locals.SetLocal(ctx.Reader.ReadShort(), ctx.Stack.Pop());

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;
    }
}