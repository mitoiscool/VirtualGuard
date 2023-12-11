using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.Reference;

namespace VirtualGuard.Runtime.OpCodes.impl
{

    public class Call : IOpCode
    {
        public void Execute(VMContext ctx, out ExecutionState state)
        {
            var methodBase = ctx.ResolveMethod(ctx.Stack.Pop().I4());


            // pop args

            var args = methodBase.GetParameters();
            var argsCasted = new object[args.Length];

            for (int i = 0; i < args.Length; i++)
            {
                // this doesn't support ref args
                argsCasted[i] = Convert.ChangeType(ctx.Stack.Pop().GetObject(), args[i].ParameterType);
            }

            throw new NotImplementedException();
            

            state = ExecutionState.Next;
        }

        public byte GetCode() => 0;

        private static Dictionary<MethodBase, DynamicMethod> _dynamicMethodCache =
            new Dictionary<MethodBase, DynamicMethod>();
    }
}