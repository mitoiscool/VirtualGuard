﻿namespace VirtualGuard.TestBinary;

public class Test
{
    public static int AddTest(int i1, int i2)
    {
        return i1 + i2;
    }
    
    public static int AddTest2(int i1, int i2)
    {
        return i1 + i2;
    }
    
    public static int AddTest3(int i1, int i2)
    {
        return i1 + i2;
    }
    
    public static int AddTest4(int i1, int i2)
    {
        return i1 + i2;
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