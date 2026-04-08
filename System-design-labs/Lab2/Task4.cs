using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static int N = 10;   // shared input value

    // Method 1: factorial of N
    static void CalcFactorial()
    {
        Console.WriteLine("[Factorial] Started.");
        long result = 1;
        for (int i = 1; i <= N; i++)
            result *= i;
        Console.WriteLine($"[Factorial] {N}! = {result}");
        Console.WriteLine("[Factorial] Completed.");
    }

    // Method 2: sum 1..N
    static void CalcSum()
    {
        Console.WriteLine("[Sum] Started.");
        long sum = 0;
        for (int i = 1; i <= N; i++)
            sum += i;
        Console.WriteLine($"[Sum] Sum of 1..{N} = {sum}");
        Console.WriteLine("[Sum] Completed.");
    }

    // Method 3: print messages with 300ms pause
    static void PrintMessages()
    {
        Console.WriteLine("[Messages] Started.");
        for (int i = 1; i <= 5; i++)
        {
            Thread.Sleep(300);
            Console.WriteLine($"[Messages] message #{i}");
        }
        Console.WriteLine("[Messages] Completed.");
    }

    static void Main()
    {
        Console.Write("Enter N: ");
        N = int.Parse(Console.ReadLine());

        Console.WriteLine("\nMain started. Running Parallel.Invoke...\n");

        // All three methods run in parallel
        // Main is blocked until ALL of them finish
        Parallel.Invoke(CalcFactorial, CalcSum, PrintMessages);

        Console.WriteLine("\nAll methods completed. Main is done.");
        Console.ReadLine();
    }
}