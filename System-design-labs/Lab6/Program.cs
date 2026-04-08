// Task 1: Directory Tree Printer
using System;
using System.IO;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            // Початкова директорія для перевірки (можна змінити на потрібну)
            string rootPath = @"C:\Users\anton\system-design-2026\System-design-labs\Lab6\TestDir";

            if (Directory.Exists(rootPath))
            {
                Console.WriteLine($"Structure of: {rootPath}");
                PrintDirectoryTree(rootPath, "", true);
            }
            else
            {
                Console.WriteLine("Directory does not exist.");
            }
        }

        static void PrintDirectoryTree(string path, string indent, bool isLast)
        {
            Console.WriteLine(indent + (isLast ? "└── " : "├── ") + Path.GetFileName(path));
            indent += isLast ? "    " : "│   ";

            try
            {
                string[] subDirs = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                for (int i = 0; i < subDirs.Length; i++)
                {
                    PrintDirectoryTree(subDirs[i], indent, i == subDirs.Length - 1 && files.Length == 0);
                }

                for (int i = 0; i < files.Length; i++)
                {
                    Console.WriteLine(indent + (i == files.Length - 1 ? "└── " : "├── ") + Path.GetFileName(files[i]));
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Обробка виключення доступу
                Console.WriteLine(indent + "└── [Access Denied]");
            }
        }
    }
}