//Task 1
using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    // Task 1: print numbers 1..10
    static void PrintNumbers()
    {
        Console.WriteLine("[Numbers] Task started. Id = " + Task.CurrentId);
        for (int i = 1; i <= 10; i++)
        {
            Console.WriteLine($"[Numbers] {i}");
            Thread.Sleep(200);
        }
        Console.WriteLine("[Numbers] Task completed.");
    }

    // Task 2: print letters A..J
    static void PrintLetters()
    {
        Console.WriteLine("[Letters] Task started. Id = " + Task.CurrentId);
        for (char c = 'A'; c <= 'J'; c++)
        {
            Console.WriteLine($"[Letters] {c}");
            Thread.Sleep(200);
        }
        Console.WriteLine("[Letters] Task completed.");
    }

    static void Main()
    {
        Console.WriteLine("Main started.");

        Task t1 = new Task(PrintNumbers);
        Task t2 = new Task(PrintLetters);

        t1.Start();
        t2.Start();

        // Wait for both tasks to finish
        Task.WaitAll(t1, t2);

        t1.Dispose();
        t2.Dispose();

        Console.WriteLine("Main completed.");
        Console.ReadLine();
    }
}