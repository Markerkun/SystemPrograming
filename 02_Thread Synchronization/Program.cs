using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;

class FileStatistics
{
    public int Words { get; set; }
    public int Lines { get; set; }
    public int Punctuation { get; set; }
}

class Program
{
    static readonly object locker = new object(); 
    static FileStatistics totalStats = new FileStatistics();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.Write("Вкажіть шлях до директорії з текстовими файлами: ");
        string path = Console.ReadLine();

        if (!Directory.Exists(path))
        {
            Console.WriteLine("Директорію не знайдено!");
            return;
        }

        string[] files = Directory.GetFiles(path, "*.txt");

        if (files.Length == 0)
        {
            Console.WriteLine("У директорії немає .txt файлів.");
            return;
        }

        List<Thread> threads = new List<Thread>();

        foreach (string file in files)
        {
            Thread t = new Thread(() => AnalyzeFile(file));
            threads.Add(t);
            t.Start();
        }

        foreach (Thread t in threads)
            t.Join();

        Console.WriteLine("\n=== Загальний результат ===");
        Console.WriteLine($"Загальна кількість слів: {totalStats.Words}");
        Console.WriteLine($"Загальна кількість рядків: {totalStats.Lines}");
        Console.WriteLine($"Загальна кількість розділових знаків: {totalStats.Punctuation}");
    }

    static void AnalyzeFile(string filePath)
    {
        string text = File.ReadAllText(filePath);
        int words = CountWords(text);
        int lines = File.ReadAllLines(filePath).Length;
        int punctuation = CountPunctuation(text);


        Console.WriteLine($"\nФайл: {Path.GetFileName(filePath)}");
        Console.WriteLine($"Слів: {words}");
        Console.WriteLine($"Рядків: {lines}");
        Console.WriteLine($"Розділових знаків: {punctuation}");


        lock (locker)
        {
            totalStats.Words += words;
            totalStats.Lines += lines;
            totalStats.Punctuation += punctuation;
        }
    }

    static int CountWords(string text)
    {
        return Regex.Matches(text, @"\b\w+\b", RegexOptions.Multiline).Count;
    }

    static int CountPunctuation(string text)
    {
        string pattern = @"[.,;:–—‒…!?\\""''«»(){}\[\]<>/]";
        return Regex.Matches(text, pattern).Count;
    }
}
