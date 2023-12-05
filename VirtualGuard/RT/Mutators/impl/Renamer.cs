namespace VirtualGuard.RT.Mutators.impl;

public class Renamer : IRuntimeMutator
{
    public void Mutate(VirtualGuardRT rt)
    {
        // rename members

        foreach (var type in rt.RuntimeModule.GetAllTypes())
        {
            type.Namespace = "vg";

            foreach (var method in type.Methods)
            {
                foreach (var def in method.ParameterDefinitions)
                    def.Name = "vg";
                
                
            }
            
            foreach (var field in type.Fields)
            {
                
            }
            
        }
        
    }
    
    
    
}