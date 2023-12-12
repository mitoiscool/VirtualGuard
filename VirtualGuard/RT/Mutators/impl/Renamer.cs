namespace VirtualGuard.RT.Mutators.impl;

public class Renamer : IRuntimeMutator
{
    private Dictionary<string, string> _abstractNameMap = new Dictionary<string, string>();
    public void Mutate(VirtualGuardRT rt, VirtualGuardContext ctx)
    {
        if(rt.isDebug)
            return;
        
        var random = new Random();
        // rename members

        foreach (var type in rt.RuntimeModule.GetAllTypes())
        {
            type.Namespace = "";
            type.Name = random.Next(int.MaxValue).ToString("x");

            foreach (var method in type.Methods)
            {
                foreach (var def in method.ParameterDefinitions)
                    def.Name = random.Next(int.MaxValue).ToString("x");

                if(method.IsConstructor || method.IsRuntimeSpecialName)
                    continue;
                
                if (method.IsAbstract)
                {
                    if (!_abstractNameMap.ContainsKey(method.Name))
                    {
                        string name = random.Next(int.MaxValue).ToString("x");
                        _abstractNameMap.Add(method.Name, name);
                        method.Name = name;
                        
                    }
                }
                else
                {
                    if (_abstractNameMap.TryGetValue(method.Name, out string sharedName))
                    { // rename abstract shared
                        method.Name = sharedName;
                    }
                    else
                    { // rename normally
                        method.Name = random.Next(int.MaxValue).ToString("x");
                    }
                    
                }


            }
            
            foreach (var field in type.Fields)
            {
                field.Name = "";
            }
            
        }
        
    }
    
}