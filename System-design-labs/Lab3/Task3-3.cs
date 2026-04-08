using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lab3
{
    class Program
    {
        static double[] data;

        static void MyTransform(double v, ParallelLoopState pls)
        {
            if (v < 0)
            {
                Console.WriteLine($"Negative value found: {v}");
                pls.Break();
                return;
            }
            double _ = v / 10.0;
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Processors: {Environment.ProcessorCount}\n");

            // --- Example 1: ForEach with Break on negative value ---
            Console.WriteLine("--- ForEach with Break on negative value ---");

            int size = 1_000_000;
            data = new double[size];

            for (int i = 0; i < size; i++)
                data[i] = i;

            data[100_000] = -10;

            var sw = Stopwatch.StartNew();
            ParallelLoopResult loopResult = Parallel.ForEach(data, MyTransform);
            sw.Stop();

            if (!loopResult.IsCompleted)
                Console.WriteLine($"ForEach aborted. LowestBreakIteration = {loopResult.LowestBreakIteration}");
            else
                Console.WriteLine("ForEach completed normally.");

            Console.WriteLine($"Time: {sw.Elapsed.TotalSeconds:F4} s\n");

            // --- Example 2: Timing comparison ---
            Console.WriteLine("--- Timing: Parallel.ForEach vs serial foreach ---");
            Console.WriteLine($"| {"Size",12} | {"Parallel(s)",12} | {"Serial(s)",12} | {"Speedup",8} |");
            Console.WriteLine(new string('-', 56));

            int[] sizes = { 1_000_000, 10_000_000, 50_000_000 };

            foreach (int n in sizes)
            {
                double[] arr = new double[n];
                double[] res = new double[n];
                for (int i = 0; i < n; i++) arr[i] = i;

                sw.Restart();
                Parallel.ForEach(arr, (v, pls, idx) =>
                {
                    res[idx] = Math.Sqrt(v * v + v) / 10.0;
                });
                sw.Stop();
                double pt = sw.Elapsed.TotalSeconds;

                sw.Restart();
                for (long i = 0; i < n; i++)
                    res[i] = Math.Sqrt(arr[i] * arr[i] + arr[i]) / 10.0;
                sw.Stop();
                double st = sw.Elapsed.TotalSeconds;

                Console.WriteLine($"| {n,12} | {pt,12:F4} | {st,12:F4} | {st / pt,8:F2}x |");
            }

            Console.ReadLine();
        }
    }
}