namespace VirtualGuard.RT.Mutators.impl;

public class Renamer : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt)
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

                if(method.IsConstructor || method.IsAbstract)
                    continue;
                
                method.Name = random.Next(int.MaxValue).ToString("x");
            }
            
            foreach (var field in type.Fields)
            {
                field.Name = "";
            }
            
        }
        
    }
    
    
    
}