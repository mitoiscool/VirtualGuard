namespace VirtualGuard.RT.Mutators.impl;

public class Renamer : IRuntimeMutator
{
    private Dictionary<string, string> _abstractNameMap = new Dictionary<string, string>();

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
                    if (!_abstractNameMap.ContainsKey(method.Name))
                    {
                        string name =
                            method.Name +
                            random.Next(int.MaxValue).ToString("x"); // Use a different prefix for abstract methods
                        _abstractNameMap.Add(method.Name, name);
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

            foreach (var field in type.Fields)
            {
                field.Name = random.Next(int.MaxValue).ToString("x"); // Use a different prefix for fields
            }
        }
    }
}