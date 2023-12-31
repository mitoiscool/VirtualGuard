

using AsmResolver.DotNet;
using Newtonsoft.Json;

namespace VirtualGuard.CLI.Config;

public class ConfigGenerator
{
    private SerializedConfig _cfg;
    private ModuleDefinition _module;
    
    public ConfigGenerator(ModuleDefinition mod)
    {
        _cfg = new SerializedConfig();
    }

    public void Populate()
    {
        
        // put default methods that people would most likely like to obfuscate in the config

        var methods = new List<MethodDefinition>();
        
        
        methods.Add(_module.ManagedEntryPointMethod);

        var largestSupportedMethods = _module.GetAllTypes()
            .SelectMany(x => x.Methods.Where(x => x.CilMethodBody != null))
            .OrderBy(x => x.CilMethodBody.Instructions.Count)
            .Where(x => x.CilMethodBody.Instructions.Count(x2 => Virtualizer.Supports(x2.OpCode)) == x.CilMethodBody.Instructions.Count).ToList();
        
        if(largestSupportedMethods.Count() > 10)
            largestSupportedMethods.RemoveRange(10, largestSupportedMethods.Count - 10); // remove all after that

        methods.AddRange(largestSupportedMethods);

        var serializedMethods = new SerializedMember[methods.Count];

        for (int i = 0; i < methods.Count; i++)
            serializedMethods[i] = new SerializedMember()
            {
                Virtualize = true,
                Exclude = false,
                Member = methods[i].DeclaringType.Namespace + "." + methods[i].DeclaringType.Name + ":" + methods[i].Name
            };

        _cfg = new SerializedConfig()
        {
            Members = serializedMethods,
            ProcessorCount = 2,
            RenameDebugSymbols = false,
            UseDataEncryption = true,
        };
        
    }


    public string Serialize()
    {
        return JsonConvert.SerializeObject(_cfg);
    }
    
    
    
    
}