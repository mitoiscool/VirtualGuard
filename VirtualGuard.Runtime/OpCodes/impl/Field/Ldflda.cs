using VirtualGuard.Runtime.Variant.Reference.impl;

namespace VirtualGuard.Runtime.OpCodes.impl.Field
{
    public class Ldflda : IOpCode
    {
        public void Execute(VMContext ctx)
        {
            var field = ctx.ResolveField(ctx.Stack.Pop().I4());

            object inst = null;

            if (!field.IsStatic)
                inst = ctx.Stack.Pop();
            
            ctx.Stack.Push(new FieldReferenceVariant(field, inst)); // no value here, interesting

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}