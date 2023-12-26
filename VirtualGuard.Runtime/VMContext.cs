using System;
using System.Collections.Generic;
using System.Reflection;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Execution;
using VirtualGuard.Runtime.OpCodes;
using VirtualGuard.Runtime.Regions;
using VirtualGuard.Runtime.Variant;
using VirtualGuard.Runtime.Variant.Object;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime
{

    public class VMContext
    {
        public VMContext(byte entryKey)
        {
            Reader.SetKey(entryKey);
        }

        public readonly VMStack Stack = new();
        public readonly LocalStorage Locals = new();
        public readonly VMReader Reader = new();

        public NumeralVariant CurrentCode;

        public Exception Exception;

        public Stack<ExceptionHandler> HandlerStack = new Stack<ExceptionHandler>();

        public object Dispatch(int loc, object[] args)
        {
            Reader.SetValue(loc);
            
            Stack.Push(new ArrayVariant(args));

            CurrentCode = Reader.ReadFixupValue();
            CodeMap.LookupCode(CurrentCode).Execute(this);
            
            return Stack.Pop().GetObject();
        }


        void Unwind()
        {
            var handler = HandlerStack.Pop();

            if (handler.Type != Constants.CatchFL)
                throw new InvalidOperationException(
                    Routines.EncryptDebugMessage("Unwinding exception handler was not a catch"));
            
            Stack.SetValue(0); // reset stack ptr
            
            // handler needs to jump to finally if we want finally to work
            
            Reader.SetKey(handler.HandlerStartKey);
            Reader.SetValue(handler.HandlerPos);
            
            Stack.Push(new ObjectVariant(Exception));
        }
        
        
        public MethodBase ResolveMethod(int i)
        {
            return (MethodBase)Assembly.GetExecutingAssembly().ManifestModule.ResolveMember(i);
        }

        public FieldInfo ResolveField(int i)
        {
            return (FieldInfo)Assembly.GetExecutingAssembly().ManifestModule.ResolveMember(i);
        }

        public Type ResolveType(int i)
        {
            return Assembly.GetExecutingAssembly().ManifestModule.ResolveType(i);
        }

        public MemberInfo ResolveMember(int i)
        { 
            return Assembly.GetExecutingAssembly().ManifestModule.ResolveMember(i);
        }

    }
}