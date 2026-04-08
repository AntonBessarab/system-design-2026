//Task1
using System;
using System.Threading;

class NumberThread
{
    public Thread Thrd;

    public NumberThread(string name)
    {
        Thrd = new Thread(Run);
        Thrd.Name = name;
    }

    public void Run()
    {
        Console.WriteLine($"[{Thrd.Name}] started.");
        for (int i = 1; i <= 40; i++)
        {
            Console.WriteLine($"[{Thrd.Name}] Number: {i}");
            Thread.Sleep(200);
        }
        Console.WriteLine($"[{Thrd.Name}] completed.");
    }
}

class LetterThread
{
    public Thread Thrd;

    public LetterThread(string name)
    {
        Thrd = new Thread(Run);
        Thrd.Name = name;
    }

    public void Run()
    {
        Console.WriteLine($"[{Thrd.Name}] started.");
        for (char c = 'A'; c <= 'Z'; c++)
        {
            Console.WriteLine($"[{Thrd.Name}] Letter: {c}");
            Thread.Sleep(300);
        }
        Console.WriteLine($"[{Thrd.Name}] completed.");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Main thread started.");

        NumberThread nt = new NumberThread("NumberThread");
        LetterThread lt = new LetterThread("LetterThread");

        nt.Thrd.Start();
        lt.Thrd.Start();

        nt.Thrd.Join();
        lt.Thrd.Join();

        Console.WriteLine("Main thread completed.");
    }
}