using VirtualGuard.Runtime.Variant;

namespace VirtualGuard.Runtime.OpCodes.impl.Field
{

    public class Ldfld : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var field = ctx.ResolveField(ctx.Stack.Pop().I4());

            object inst = null;

            if (!field.IsStatic) // ldsfld
                inst = ctx.Stack.Pop().GetObject();

            var value = BaseVariant.CreateVariant(field.GetValue(inst));

            ctx.Stack.Push(value);

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}