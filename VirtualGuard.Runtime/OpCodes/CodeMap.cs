using System;
using System.Collections.Generic;
using System.Reflection;
using VirtualGuard.Runtime.Dynamic;
using VirtualGuard.Runtime.Variant.ValueType.Numeric;

namespace VirtualGuard.Runtime.OpCodes
{

    public class CodeMap
    {
        private static Dictionary<byte, IOpCode> _opCodes = new Dictionary<byte, IOpCode>();

        static CodeMap()
        {
            _opCodes = new Dictionary<byte, IOpCode>();
            foreach (var type in typeof(CodeMap).Assembly.GetTypes())
                if (typeof(IOpCode).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    var opCode = (IOpCode)Activator.CreateInstance(type);
                    _opCodes[opCode.GetCode()] = opCode;
                }
        }

        public static IOpCode GetCode(ByteVariant code)
        {
            
            if (!_opCodes.TryGetValue(code.U1(), out IOpCode codeobj))
                throw new Exception(Routines.EncryptDebugMessage("Error resolving handler"));
            
            return codeobj;
        }
    }
}