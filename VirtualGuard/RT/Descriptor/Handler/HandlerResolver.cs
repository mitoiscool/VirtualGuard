using AsmResolver.DotNet;
using AsmResolver.DotNet.Cloning;
using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;
using VirtualGuard.RT.Dynamic;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Descriptor.Handler;

internal class HandlerResolver
{
    private static Random _rnd = new Random();
    
    private Dictionary<VmCode, TypeDefinition> _handlerDefinitions = new Dictionary<VmCode, TypeDefinition>();

    private List<VmHandler> _handlerCache = new List<VmHandler>();
    private ModuleDefinition _runtimeModule;

    public HandlerResolver(ModuleDefinition runtime)
    {
        _runtimeModule = runtime;
        
        // populate handler definitions

        var opcodes = typeof(VmCode).GetEnumNames().Where(x => x.Substring(0, 2) != "__").ToArray(); // eliminate transform instrs

        foreach (var name in opcodes)
        {
            try
            {
                var type = runtime.LookupType(RuntimeConfig.BaseHandler + "." + name);

                _handlerDefinitions.Add((VmCode)Array.IndexOf(opcodes, name), type);
                runtime.TopLevelTypes.Remove(type); // remove (will be added later)
            }
            catch(InvalidOperationException)
            {
                throw new KeyNotFoundException("Could not locate opcode: " + name + " in runtime!");
            }
        }
        
        // build handler map (we will include all handlers for this basic impl)
        
        byte[] opCodeOrder = Enumerable.Range(0, byte.MaxValue).Select(x => (byte)x).ToArray();
        _rnd.Shuffle(opCodeOrder); // make an array of random bytes

        int i = 0;
        
        do
        {
            foreach (var handlerDefKvp in _handlerDefinitions)
            {
                // add some extra randomness
                
                if(_handlerCache.Any(x => x.OpCode == handlerDefKvp.Key) && _rnd.Next(9) == 0) 
                    continue; // potentially randomly stop execution if the opcode has already been added (1/10)
                
                 // clone type def

                 var oldType = handlerDefKvp.Value;

                 var newDefinition = oldType.Clone(_runtimeModule);

                 _handlerCache.Add(new VmHandler(handlerDefKvp.Key, newDefinition, opCodeOrder[i++]));
                _runtimeModule.TopLevelTypes.Add(newDefinition);
                
                if(i == opCodeOrder.Length)
                    break; // don't let it go over
            }
            
        } while (i != opCodeOrder.Length);
        
        // pray this works
    }

    public VmHandler GetHandler(VmCode code)
    { // get a random handler that for the opcode
        var handlersSupportingCode = _handlerCache.Where(x => x.OpCode == code).ToArray();

        return handlersSupportingCode[_rnd.Next(handlersSupportingCode.Length)];
    }


}