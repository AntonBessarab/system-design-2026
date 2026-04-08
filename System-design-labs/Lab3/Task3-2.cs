using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lab3
{
    class Program
    {
        const double TARGET = 500_000.0;
        const double DEVIATION = 50.0;

        static double[] data;

        static void ProcessElement(int i, ParallelLoopState pls)
        {
            if (pls.ShouldExitCurrentIteration)
                return;

            double val = data[i] / 10.0;

            if (Math.Abs(val - TARGET) <= DEVIATION)
            {
                Console.WriteLine($"Found: data[{i}] = {data[i]}, transformed = {val:F2}, deviation = {Math.Abs(val - TARGET):F2}");
                pls.Break();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Target: {TARGET},  Deviation: +/-{DEVIATION}");
            Console.WriteLine($"Search range: [{TARGET - DEVIATION}, {TARGET + DEVIATION}]\n");

            int size = 10_000_000;
            data = new double[size];

            for (int i = 0; i < size; i++)
                data[i] = i;

            var sw = Stopwatch.StartNew();
            ParallelLoopResult result = Parallel.For(0, data.Length, ProcessElement);
            sw.Stop();

            if (!result.IsCompleted)
            {
                Console.WriteLine($"\nLoop aborted. LowestBreakIteration = {result.LowestBreakIteration}");
                long idx = result.LowestBreakIteration ?? -1;
                if (idx >= 0)
                    Console.WriteLine($"data[{idx}] = {data[idx]}, transformed = {data[idx] / 10.0:F2}");
            }
            else
            {
                Console.WriteLine("Loop completed normally, no value found in range.");
            }

            Console.WriteLine($"Parallel time: {sw.Elapsed.TotalSeconds:F4} s\n");

            Console.WriteLine("--- Serial search ---");
            sw.Restart();
            for (int i = 0; i < data.Length; i++)
            {
                double val = data[i] / 10.0;
                if (Math.Abs(val - TARGET) <= DEVIATION)
                {
                    Console.WriteLine($"Found: data[{i}] = {data[i]}, transformed = {val:F2}");
                    break;
                }
            }
            sw.Stop();
            Console.WriteLine($"Serial time: {sw.Elapsed.TotalSeconds:F4} s");

            Console.ReadLine();
        }
    }
}