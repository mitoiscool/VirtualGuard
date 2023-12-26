using System;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Stloc : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            ctx.Locals.SetLocal(ctx.Reader.ReadShort(), ctx.Stack.Pop());

            ctx.CurrentCode = ctx.CurrentCode.Add(ctx.Reader.ReadFixupValue().ToNumeral());
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}