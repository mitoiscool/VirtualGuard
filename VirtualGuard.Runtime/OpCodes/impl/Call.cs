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
        public void Execute(VMContext ctx)
        {
            var methodBase = ctx.ResolveMethod(ctx.Reader.ReadInt());
            
            var args = methodBase.GetParameters();
            var vmVariantArgs = new BaseVariant[args.Length];
            var argsCasted = new object[args.Length];

            var refIndexes = new HashSet<int>();
            
            for (int i = args.Length - 1; i >= 0; i--)
            {
                vmVariantArgs[i] = ctx.Stack.Pop();

                if (vmVariantArgs[i].IsReference())
                    refIndexes.Add(i); // mark as reference to set value to after

                if (vmVariantArgs[i].GetObject().GetType() is IConvertible conv)
                {
                    argsCasted[i] = Convert.ChangeType(conv, args[i].ParameterType);
                }
                else
                {
                    argsCasted[i] = vmVariantArgs[i].GetObject();
                }
            }

            // calculate inst
            
            object inst = null;
            bool isInstReference = false;
            BaseVariant instVariant = null;

            if (!methodBase.IsStatic)
            { // if it is inst
                
                instVariant = ctx.Stack.Pop();

                if (instVariant.IsReference())
                    isInstReference = true;
                
                // get object for inst

                inst = instVariant.GetObject();
            }

            object ret = methodBase.Invoke(inst, argsCasted);
            
            // update ref vars
            foreach (var refIndex in refIndexes) 
                vmVariantArgs[refIndex].SetVariantValue(argsCasted[refIndex]); // set value of associated reference variant to that of the new variable
            
            if(isInstReference)
                instVariant.SetVariantValue(inst); // update inst ref
            
            if(methodBase.IsConstructor || methodBase is MethodInfo mi && mi.ReturnType != typeof(void))
                ctx.Stack.Push(BaseVariant.CreateVariant(ret));

            ctx.CurrentCode += ctx.Reader.ReadFixupValue();
            CodeMap.LookupCode(ctx.CurrentCode).Execute(ctx);
        }

        public byte GetCode() => 0;
    }
}