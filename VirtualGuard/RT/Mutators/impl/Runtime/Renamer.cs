using AsmResolver.DotNet.Signatures;

namespace VirtualGuard.RT.Mutators.impl;

internal class Renamer : IRuntimeMutator
{
    private Dictionary<string, string> _abstractNameMap = new Dictionary<string, string>();

    private static int Vm = 0; // scuffed but easy
    
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        if(rt.isDebug)
            return;

        var random = new Random();

        // establish names for all abstract methods
        foreach (var type in rt.RuntimeModule.GetAllTypes().Where(x => !x.IsModuleType && !x.IsRuntimeSpecialName))
        {
            foreach (var method in type.Methods)
            {

                if (method.IsAbstract || method.IsVirtual || method.DeclaringType.IsInterface && method.CilMethodBody != null && method.CilMethodBody.Instructions.Count == 0)
                {
                    if (!_abstractNameMap.ContainsKey(method.Name))
                    {
                        string name = random.Next(int.MaxValue).ToString("x"); // Use a different prefix for abstract methods
                        _abstractNameMap.Add(method.Name, name);
                        method.Name = name;
                    }
                }
            }
        }


        foreach (var type in rt.RuntimeModule.GetAllTypes().Where(x => !x.IsModuleType && !x.IsRuntimeSpecialName))
        {
            type.Namespace = random.Next(int.MaxValue).ToString("x");
            type.Name = "vg" + random.Next(int.MaxValue).ToString("x");

            foreach (var method in type.Methods)
            {
                foreach (var def in method.ParameterDefinitions)
                    def.Name = random.Next(int.MaxValue).ToString("x");

                if (method.IsConstructor || method.IsRuntimeSpecialName)
                    continue;

                if (_abstractNameMap.TryGetValue(method.Name, out string sharedName))
                {
                    // rename abstract shared
                    method.Name = sharedName;
                }
                else
                {
                    if(method.IsAbstract || method.IsVirtual)
                        continue;
                    
                    // rename normally
                    method.Name =
                        random.Next(int.MaxValue)
                           .ToString("x"); // Use a different prefix for non-abstract methods
                }
            }

            var fieldName = random.Next(int.MaxValue)
                .ToString("x");
            
            foreach (var field in type.Fields)
            {
                field.Name = fieldName; // Use a different prefix for fields
            }
        }
    }
}

