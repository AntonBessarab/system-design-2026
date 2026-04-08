using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            // Запитуємо шлях у користувача
            Console.Write("Enter search directory path: ");
            string searchDirectory = Console.ReadLine();

            // Запитуємо назву файлу або маску у користувача
            Console.Write("Enter file name or mask to search (e.g., target.txt or *.txt): ");
            string searchFileName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchDirectory) || string.IsNullOrWhiteSpace(searchFileName))
            {
                Console.WriteLine("Invalid input. Path and file name cannot be empty.");
                return;
            }

            if (Directory.Exists(searchDirectory))
            {
                Console.WriteLine($"\nSearching for '{searchFileName}' in '{searchDirectory}'...\n");
                SearchFile(searchDirectory, searchFileName);
            }
            else
            {
                Console.WriteLine("Search directory does not exist.");
            }
        }

        static void SearchFile(string directory, string fileName)
        {
            try
            {
                // Шукаємо файли в поточній директорії
                string[] files = Directory.GetFiles(directory, fileName);
                foreach (string file in files)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    Console.WriteLine($"File Found: {fileInfo.FullName}");
                    Console.WriteLine($"Size: {fileInfo.Length} bytes");
                    Console.WriteLine($"Created: {fileInfo.CreationTime}");
                    Console.WriteLine($"Last Modified: {fileInfo.LastWriteTime}");
                    Console.WriteLine(new string('-', 40));
                }

                // Рекурсивно шукаємо в піддиректоріях
                string[] subDirs = Directory.GetDirectories(directory);
                foreach (string subDir in subDirs)
                {
                    SearchFile(subDir, fileName);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Пропускаємо директорії, до яких немає доступу
            }
        }
    }
}
