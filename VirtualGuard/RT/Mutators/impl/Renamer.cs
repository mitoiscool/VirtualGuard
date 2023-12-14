using AsmResolver.DotNet.Signatures;

namespace VirtualGuard.RT.Mutators.impl;

public class Renamer : IRuntimeMutator
{
    private Dictionary<MethodSignature, string> _abstractNameMap = new Dictionary<MethodSignature, string>();

    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        if (rt.isDebug)
            return;

        var random = new Random();

        // establish names for all abstract methods
        foreach (var type in rt.RuntimeModule.GetAllTypes())
        {
            foreach (var method in type.Methods)
            {

                if (method.IsAbstract || method.IsVirtual)
                {
                    if (!_abstractNameMap.ContainsKey(method.Signature))
                    {
                        string name =
                            method.Name +
                            random.Next(int.MaxValue).ToString("x"); // Use a different prefix for abstract methods
                        _abstractNameMap.Add(method.Signature, name);
                        method.Name = name;
                    }
                }
            }
        }


        foreach (var type in rt.RuntimeModule.GetAllTypes())
        {
            type.Namespace = "vg";
            type.Name = "vg" + random.Next(int.MaxValue).ToString("x");

            foreach (var method in type.Methods)
            {
                foreach (var def in method.ParameterDefinitions)
                    def.Name = "vg" + random.Next(int.MaxValue).ToString("x");

                if (method.IsConstructor || method.IsRuntimeSpecialName)
                    continue;

                if (_abstractNameMap.TryGetValue(method.Signature, out string sharedName))
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

            foreach (var field in type.Fields)
            {
                field.Name = random.Next(int.MaxValue).ToString("x"); // Use a different prefix for fields
            }
        }
    }
}