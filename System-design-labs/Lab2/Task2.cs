using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void CountTask()
    {
        int taskId = Task.CurrentId ?? -1;
        Console.WriteLine($"Task started   | Id = {taskId}");

        for (int i = 1; i <= 5; i++)
        {
            Thread.Sleep(300);
            Console.WriteLine($"  [CurrentId = {Task.CurrentId}] count = {i}");
        }

        Console.WriteLine($"Task completed | Id = {taskId}");
    }

    static void Main()
    {
        Console.WriteLine("Main started.");

        Task t1 = Task.Factory.StartNew(CountTask);
        Task t2 = Task.Factory.StartNew(CountTask);
        Task t3 = Task.Factory.StartNew(CountTask);

        // Print assigned Ids from the outside
        Console.WriteLine($"t1.Id = {t1.Id}");
        Console.WriteLine($"t2.Id = {t2.Id}");
        Console.WriteLine($"t3.Id = {t3.Id}");

        Task.WaitAll(t1, t2, t3);

        t1.Dispose();
        t2.Dispose();
        t3.Dispose();

        Console.WriteLine("Main completed.");
        Console.ReadLine();
    }
}