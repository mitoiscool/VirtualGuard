using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.Exports;
using Echo.Platforms.AsmResolver;

namespace VirtualGuard.RT.Dynamic;

internal class MutationExpression
{
    public List<MutationStep> Steps = new List<MutationStep>();


    static Random _rnd = new Random();
    
    public static MutationExpression Random(int steps)
    {
        var expr = new MutationExpression();

        for (int i = 0; i < steps; i++)
        {
            // get random operation and mod
            var modifier = _rnd.Next(1, 100000);
            var operation = (Operation)_rnd.Next(typeof(Operation).GetEnumNames().Length);
            //var operation = (Operation)_rnd.Next(3); // hardcode debug
            
            expr.Steps.Add(new MutationStep(modifier, operation));
        }

        return expr;
    }

    public int Solve(int input)
    {
        foreach (var step in this.Steps.ToArray().Reverse())
        {
            input = step.Operation.Operate(input, step.Modifier);
        }

        return input;
    }

    public CilInstruction[] ToCIL()
    {
        // fml this is a headache
        var instrs = new List<CilInstruction>();

        foreach (var step in Steps)
        {
            instrs.Add(new CilInstruction(CilOpCodes.Ldc_I4, step.Modifier));
            instrs.Add(new CilInstruction(step.Operation.Inverse().ToCil()));
        }
        
        instrs.Add(new CilInstruction(CilOpCodes.Conv_U1));

        return instrs.ToArray();
    }
    
}