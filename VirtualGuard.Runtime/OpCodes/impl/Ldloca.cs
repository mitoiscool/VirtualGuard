using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant.Reference.impl;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Ldloca : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var local = ctx.Locals.GetLocal(ctx.Reader.ReadShort());
            
            ctx.Stack.Push(new LocalReferenceVariant(local));

            ctx.CurrentCode = ctx.CurrentCode.Add(ctx.Reader.ReadFixupValue().ToNumeral());
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
        
    }
}