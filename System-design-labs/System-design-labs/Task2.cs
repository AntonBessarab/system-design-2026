using System;
using System.Threading;

class ForegroundThread
{
    public Thread Thrd;
    private int _count;

    public ForegroundThread(string name, int count)
    {
        _count = count;
        Thrd = new Thread(Run);
        Thrd.Name = name;
        // IsBackground = false by default (foreground)
        Thrd.IsBackground = false;
    }

    public void Run()
    {
        Console.WriteLine($"[{Thrd.Name}] (Foreground) started.");
        for (int i = 1; i <= _count; i++)
        {
            Console.WriteLine($"[{Thrd.Name}] step {i}/{_count}");
            Thread.Sleep(500);
        }
        Console.WriteLine($"[{Thrd.Name}] (Foreground) completed.");
    }
}

class BackgroundWorker
{
    public Thread Thrd;

    public BackgroundWorker(string name)
    {
        Thrd = new Thread(Run);
        Thrd.Name = name;
        // Make this thread background
        Thrd.IsBackground = true;
    }

    public void Run()
    {
        Console.WriteLine($"[{Thrd.Name}] (Background) started.");
        int tick = 0;
        while (true)  // infinite loop
        {
            Console.WriteLine($"[{Thrd.Name}] (Background) working... tick {++tick}");
            Thread.Sleep(700);
        }
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Main thread started.");

        ForegroundThread ft1 = new ForegroundThread("Foreground-1", 5);
        ForegroundThread ft2 = new ForegroundThread("Foreground-2", 4);
        BackgroundWorker bg = new BackgroundWorker("Background-1");

        ft1.Thrd.Start();
        ft2.Thrd.Start();
        bg.Thrd.Start();

        ft1.Thrd.Join();
        ft2.Thrd.Join();

        // After both foreground threads finish,
        // the background thread is killed automatically.
        Console.WriteLine("All foreground threads done. Program will exit now.");
        Console.WriteLine("Background thread was terminated automatically.");
    }
}