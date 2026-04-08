//task 1
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lab3
{
    class Program
    {
        static double ComputeDouble(double x, int complexity)
        {
            switch (complexity)
            {
                case 1: return x / 10.0;
                case 2: return x * x;
                case 3: return Math.Sqrt(x * x + x);
                case 4: return Math.Exp(x % 10) * Math.Exp(-(x % 10));
                default: return x;
            }
        }

        static int ComputeInt(int x, int complexity)
        {
            switch (complexity)
            {
                case 1: return x / 10;
                case 2: return x * x % 100000;
                case 3: return (int)Math.Sqrt(Math.Abs((long)x * x + x));
                case 4: return (int)(Math.Exp(x % 10) * Math.Exp(-(x % 10)));
                default: return x;
            }
        }

        static void RunDoubleExperiment(int size, int complexity)
        {
            double[] data = new double[size];
            double[] result = new double[size];

            for (int i = 0; i < size; i++)
                data[i] = i * 0.001;

            var sw = Stopwatch.StartNew();
            Parallel.For(0, size, i => { result[i] = ComputeDouble(data[i], complexity); });
            sw.Stop();
            double parallelTime = sw.Elapsed.TotalSeconds;

            sw.Restart();
            for (int i = 0; i < size; i++)
                result[i] = ComputeDouble(data[i], complexity);
            sw.Stop();
            double serialTime = sw.Elapsed.TotalSeconds;

            Console.WriteLine($"| double | {size,12} | {complexity} | {parallelTime,10:F4} | {serialTime,10:F4} | {serialTime / parallelTime,7:F2}x |");
        }

        static void RunIntExperiment(int size, int complexity)
        {
            int[] data = new int[size];
            int[] result = new int[size];

            for (int i = 0; i < size; i++)
                data[i] = i;

            var sw = Stopwatch.StartNew();
            Parallel.For(0, size, i => { result[i] = ComputeInt(data[i], complexity); });
            sw.Stop();
            double parallelTime = sw.Elapsed.TotalSeconds;

            sw.Restart();
            for (int i = 0; i < size; i++)
                result[i] = ComputeInt(data[i], complexity);
            sw.Stop();
            double serialTime = sw.Elapsed.TotalSeconds;

            Console.WriteLine($"| int    | {size,12} | {complexity} | {parallelTime,10:F4} | {serialTime,10:F4} | {serialTime / parallelTime,7:F2}x |");
        }

        static void Main(string[] args)
        {
            Console.WriteLine($"Processors: {Environment.ProcessorCount}");
            Console.WriteLine("Complexity: 1=x/10  2=x*x  3=sqrt(x*x+x)  4=exp(x)*exp(-x)\n");
            Console.WriteLine($"| Type   | {"Size",12} | Cmp | {"Parallel(s)",10} | {"Serial(s)",10} | {"Speedup",7} |");
            Console.WriteLine(new string('-', 72));

            int[] sizes = { 100_000, 1_000_000, 10_000_000, 50_000_000 };
            int[] complexities = { 1, 2, 3, 4 };

            foreach (int size in sizes)
                foreach (int c in complexities)
                {
                    RunDoubleExperiment(size, c);
                    RunIntExperiment(size, c);
                }

            Console.WriteLine(new string('-', 72));
            Console.ReadLine();
        }
    }
}