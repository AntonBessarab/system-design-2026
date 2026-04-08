using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3
{
    class Program
    {
        const double TARGET = 50_000.0;
        const double DEVIATION = 10.0;

        static void Main(string[] args)
        {
            Console.WriteLine($"Processors: {Environment.ProcessorCount}");
            Console.WriteLine($"Break condition: |transformed - {TARGET}| <= {DEVIATION}\n");

            int size = 2_000_000;
            double[] data = new double[size];
            double[] result = new double[size];

            for (int i = 0; i < size; i++)
                data[i] = i;

            // --- Example 1: simple lambda, no Break ---
            Console.WriteLine("--- Simple lambda (no Break) ---");
            var sw = Stopwatch.StartNew();

            Parallel.ForEach(data, (v, pls, idx) =>
            {
                result[idx] = v / 10.0;
            });

            sw.Stop();
            Console.WriteLine($"Time: {sw.Elapsed.TotalSeconds:F4} s");
            Console.WriteLine($"result[0]={result[0]}, result[500000]={result[500000]}\n");

            // --- Example 2: lambda with Break when value enters neighborhood ---
            Console.WriteLine("--- Lambda with Break (neighborhood condition) ---");
            long foundIndex = -1;

            sw.Restart();
            ParallelLoopResult loopResult = Parallel.ForEach(
                data,
                (double v, ParallelLoopState pls, long idx) =>
                {
                    if (pls.ShouldExitCurrentIteration)
                        return;

                    double transformed = v / 10.0;

                    if (Math.Abs(transformed - TARGET) <= DEVIATION)
                    {
                        Interlocked.CompareExchange(ref foundIndex, idx, -1);
                        Console.WriteLine($"Found: idx={idx}, data={v}, transformed={transformed:F2}, deviation={Math.Abs(transformed - TARGET):F2}");
                        pls.Break();
                    }
                }
            );
            sw.Stop();

            if (!loopResult.IsCompleted)
                Console.WriteLine($"Loop aborted. LowestBreakIteration = {loopResult.LowestBreakIteration}");
            else
                Console.WriteLine("Loop completed normally, nothing found.");

            Console.WriteLine($"Time: {sw.Elapsed.TotalSeconds:F4} s\n");

            // --- Example 3: timing comparison with lambda ---
            Console.WriteLine("--- Timing: lambda ForEach vs serial loop ---");
            Console.WriteLine($"| {"Size",12} | {"Parallel(s)",12} | {"Serial(s)",12} | {"Speedup",8} |");
            Console.WriteLine(new string('-', 56));

            int[] sizes = { 5_000_000, 20_000_000, 50_000_000 };

            foreach (int n in sizes)
            {
                double[] arr = new double[n];
                double[] res = new double[n];
                for (int i = 0; i < n; i++) arr[i] = i;

                sw.Restart();
                Parallel.ForEach(arr, (v, pls, idx) =>
                {
                    res[idx] = Math.Sqrt(v) + Math.Log(v + 1.0);
                });
                sw.Stop();
                double pt = sw.Elapsed.TotalSeconds;

                sw.Restart();
                for (long i = 0; i < n; i++)
                    res[i] = Math.Sqrt(arr[i]) + Math.Log(arr[i] + 1.0);
                sw.Stop();
                double st = sw.Elapsed.TotalSeconds;

                Console.WriteLine($"| {n,12} | {pt,12:F4} | {st,12:F4} | {st / pt,8:F2}x |");
            }

            Console.ReadLine();
        }
    }
}