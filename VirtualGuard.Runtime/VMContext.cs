using System;
using System.Collections.Generic;
using System.Reflection;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.OpCodes;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;

namespace VirtualGuard.Runtime
{

    public class VMContext
    {
        public VMContext(byte entryKey)
        {
            _entryKey = entryKey;
        }

        private byte _entryKey;
        public readonly VMStack Stack = new();
        public readonly LocalStorage Locals = new();
        public readonly VMReader Reader = new();

        private Exception _exception;

        public object Dispatch(int loc, object[] args)
        {
            Reader.SetValue(loc);
            Reader.SetKey(_entryKey);
            
            Stack.Push(new ArrayVariant(args));

            switch (DispatchInternal())
            {
                case ExecutionState.Catch:
                    // handle _exception, check for finally and potentially jump to that state in the case

                    // no exception handling yet, throw

                    throw _exception;
                    
                    // if finally, goto case finally
                    break;

                case ExecutionState.Finally:
                    // jump to handler location 
                    break;
            }

            return Stack.Pop();
        }

        ExecutionState DispatchInternal()
        {
            ExecutionState state;

            do
            {
                //try
                //{
                    var handler = Reader.ReadHandler();

                    //Console.WriteLine(CodeMap.LookupCode(handler).GetType().Name);
                    CodeMap.LookupCode(handler).Execute(this, out state);
                    

                    if (state != ExecutionState.Next)
                        break;

                //}
                //catch (Exception ex)
                //{
                //    _exception = ex; // warning to self, this may suppress vm exceptions
                //    return ExecutionState.Catch;
                //}

            } while (true);

            return state;
        }

        public MethodBase ResolveMethod(int i)
        {
            return (MethodBase)Assembly.GetExecutingAssembly().ManifestModule.ResolveMember(i);
        }

        public FieldInfo ResolveField(int i)
        {
            return (FieldInfo)Assembly.GetExecutingAssembly().ManifestModule.ResolveMember(i);
        }

    }
}