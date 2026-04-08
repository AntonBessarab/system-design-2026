using System;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        Console.WriteLine("Main started.");

        Console.Write("Enter N: ");
        int n = int.Parse(Console.ReadLine());

        // Task 1: compute sum 1..N, returns int
        Task<int> sumTask = Task<int>.Factory.StartNew(() =>
        {
            Console.WriteLine($"[SumTask] Id = {Task.CurrentId} | Computing sum 1..{n}...");
            int sum = 0;
            for (int i = 1; i <= n; i++)
                sum += i;
            Console.WriteLine($"[SumTask] Id = {Task.CurrentId} | Done.");
            return sum;
        });

        // Continuation (lambda): runs automatically after sumTask finishes
        Task continuation = sumTask.ContinueWith((prevTask) =>
        {
            Console.WriteLine($"[Continuation] Id = {Task.CurrentId} | Started.");
            Console.WriteLine($"[Continuation] Sum of 1..{n} = {prevTask.Result}");
            Console.WriteLine($"[Continuation] Id = {Task.CurrentId} | Completed.");
        });

        // Wait for the full chain to finish
        continuation.Wait();

        sumTask.Dispose();
        continuation.Dispose();

        Console.WriteLine("Main completed.");
        Console.ReadLine();
    }
}