using System;
using System.Threading;

class CounterThread
{
    public long Count;
    public Thread Thrd;
    public ThreadPriority SavedPriority;  // save priority here
    static volatile bool stop = false;
    private const long Limit = 100_000_000;

    public CounterThread(string name, ThreadPriority priority)
    {
        Count = 0;
        SavedPriority = priority;  // store before thread dies
        Thrd = new Thread(Run);
        Thrd.Name = name;
        Thrd.Priority = priority;
    }

    public void Run()
    {
        Console.WriteLine($"[{Thrd.Name}] ({SavedPriority}) started.");
        while (!stop && Count < Limit)
        {
            Count++;
        }
        stop = true;
        Console.WriteLine($"[{Thrd.Name}] ({SavedPriority}) completed. Count = {Count}");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Task 3 | Variant 1 ===");
        Console.WriteLine("3 Threads: Normal, AboveNormal, Highest\n");

        CounterThread t1 = new CounterThread("Thread-Normal", ThreadPriority.Normal);
        CounterThread t2 = new CounterThread("Thread-AboveNormal", ThreadPriority.AboveNormal);
        CounterThread t3 = new CounterThread("Thread-Highest", ThreadPriority.Highest);

        t1.Thrd.Start();
        t2.Thrd.Start();
        t3.Thrd.Start();

        t1.Thrd.Join();
        t2.Thrd.Join();
        t3.Thrd.Join();

        long total = t1.Count + t2.Count + t3.Count;

        Console.WriteLine("\n=== RESULTS ===");
        Console.WriteLine($"{"Thread",-22} {"Priority",-15} {"Count",15} {"CPU %",10}");
        Console.WriteLine(new string('-', 65));

        void PrintResult(CounterThread t)
        {
            double pct = total > 0 ? (double)t.Count / total * 100.0 : 0;
            // Use SavedPriority instead of Thrd.Priority
            Console.WriteLine($"{t.Thrd.Name,-22} {t.SavedPriority,-15} {t.Count,15} {pct,9:F2}%");
        }

        PrintResult(t1);
        PrintResult(t2);
        PrintResult(t3);

        Console.WriteLine(new string('-', 65));
        Console.WriteLine($"{"TOTAL",-22} {"",-15} {total,15} {"100.00%",10}");
        Console.ReadLine();
    }
}