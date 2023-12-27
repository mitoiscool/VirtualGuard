using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant.Reference.impl;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Ldelema : IOpCode
    {
        public void Execute(VMContext ctx)
        { // need to make arrayreferencevariant

            var delem = ctx.Stack.Pop();
            var arr = ctx.Stack.Pop().ToArray();

            ctx.Stack.Push(new ArrayReferenceVariant(arr, delem));
            
            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}