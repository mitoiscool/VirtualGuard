using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.ValueType;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Ldstr : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var res = ctx.Reader.ReadString((uint)ctx.Reader.ReadInt());
            
            if(res == null)
                ctx.Stack.Push(new NullVariant());
            else
                ctx.Stack.Push(new StringVariant(res));

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}