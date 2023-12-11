namespace VirtualGuard.TestBinary;

public class Test
{
    public static int AddTest(int i1, int i2)
    {
        return i1 + i2;
    }
    
    public static void CallTest()
    {
        ConditionalTest();
        
        Console.WriteLine(AddTest(10, 12));
        Console.WriteLine(Console.ReadLine());
        Console.WriteLine("done");
    }

    public static void ConditionalTest()
    {
        if (Console.ReadKey().KeyChar == 'a')
        {
            Console.WriteLine("true");
        }
        else
        {
            Console.WriteLine("false");
        }
    }

    static void AdvancedTest()
    {
        // Initialize some variables
        int a = 10;
        int b = 5;

        // Arithmetic operations
        int resultAdd = a + b;
        int resultSub = a - b;
        int resultMul = a * b;
        int resultDiv = a / b;

        // Bitwise operations
        int resultAnd = a & b;
        int resultOr = a | b;
        int resultXor = a ^ b;

        // Display results
        Console.WriteLine("Arithmetic operations:");
        Console.WriteLine($"{a} + {b} = {resultAdd}");
        Console.WriteLine($"{a} - {b} = {resultSub}");
        Console.WriteLine($"{a} * {b} = {resultMul}");
        Console.WriteLine($"{a} / {b} = {resultDiv}");

        Console.WriteLine("\nBitwise operations:");
        Console.WriteLine($"{a} & {b} = {resultAnd}");
        Console.WriteLine($"{a} | {b} = {resultOr}");
        Console.WriteLine($"{a} ^ {b} = {resultXor}");

        // Advanced sequence: Loop
        Console.WriteLine("\nLoop example:");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"Iteration {i + 1}");
        }

        // Advanced sequence: Conditional statement
        Console.WriteLine("\nConditional statement example:");
        if (a > b)
        {
            Console.WriteLine($"{a} is greater than {b}");
        }
        else
        {
            Console.WriteLine($"{a} is not greater than {b}");
        }
    }

    public static void TestCaller1()
    {
        AddTest(10, 12);
    }
    public static void TestCaller2()
    {
        AddTest(10, 12);
    }
}