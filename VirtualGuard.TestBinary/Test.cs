namespace VirtualGuard.TestBinary;

public class Test
{
    public static int AddTest(int i1, int i2)
    {
        return i1 + i2;
    }
    
    public static void CallTest()
    {
        Console.WriteLine("test1");
        Console.WriteLine("test1");
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


    public static void TestCaller1()
    {
        AddTest(10, 12);
    }
    public static void TestCaller2()
    {
        AddTest(10, 12);
    }
}