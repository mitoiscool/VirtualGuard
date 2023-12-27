using VirtualGuard.Runtime.Execution;

namespace VirtualGuard.Runtime.OpCodes.impl
{
    public class Ldelem : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var delem = ctx.Stack.Pop();
            var arr = ctx.Stack.Pop().ToArray();
            
            ctx.Stack.Push(arr.LoadDelimeter(delem));

            ctx.CurrentCode = ctx.CurrentCode.Add(ctx.Reader.ReadFixupValue().ToNumeral());
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}