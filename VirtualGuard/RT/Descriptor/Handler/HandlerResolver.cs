using AsmResolver.DotNet;
using VirtualGuard.VMIL.VM;

namespace VirtualGuard.RT.Descriptor.Handler;

public class HandlerResolver
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
        opcodes.Shuffle(); // shuffle the values before looking up just to ensure the definition array is unique
        
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
        
        byte[] opCodeOrder = Enumerable.Range(0, 256).Select(x => (byte)x).ToArray();
        _rnd.Shuffle(opCodeOrder); // make an array of random bytes

        int i = 0;
        
        do
        {
            foreach (var handlerDefKvp in _handlerDefinitions)
            {
                // add some extra randomness
                
                if(_handlerCache.Any(x => x.OpCode == handlerDefKvp.Key) && _rnd.Next(9) == 0)
                    continue; // potentially randomly stop execution if the opcode has already been added (1/10)
                
                _handlerCache.Add(new VmHandler(handlerDefKvp.Key, handlerDefKvp.Value, opCodeOrder[i++]));
                
                if(i == byte.MaxValue)
                    break; // don't let it go over
            }
            
        } while (true);
        
        // pray this works
    }

    public VmHandler GetHandler(VmCode code)
    { // will get essentially a random handler
        var handlersSupportingCode = _handlerCache.Where(x => x.OpCode == code).ToArray();

        return handlersSupportingCode[_rnd.Next(handlersSupportingCode.Length)];
    }

    public void CommitHandlers()
    {
        foreach (var handler in _handlerCache)
        {
            _runtimeModule.TopLevelTypes.Add(handler.HandlerDefinition);
        }
    }
    
}