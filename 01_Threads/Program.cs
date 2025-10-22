using System;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    //1
    static void PrintNumbers()
    {
        for (int i = 0; i <= 50; i++)
        {
            Console.WriteLine(i);
            Thread.Sleep(50); 
        }
    }



    //2
    static void PrintNumbers(int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            Console.WriteLine(i);
            Thread.Sleep(50);
        }
    }

    //4
    static int[] numbers = new int[10000];
    static int min, max;
    static double avg;

    static void FindMin() => min = numbers.Min();
    static void FindMax() => max = numbers.Max();
    static void FindAverage() => avg = numbers.Average();




    //5
    static void WriteToFile()
    {
        using (StreamWriter sw = new StreamWriter("results.txt"))
        {
            sw.WriteLine("Згенеровані числа:");
            foreach (var num in numbers)
                sw.Write(num + " ");

            sw.WriteLine("\n\nРезультати:");
            sw.WriteLine($"Min: {min}");
            sw.WriteLine($"Max: {max}");
            sw.WriteLine($"Average: {avg:F2}");
        }
    }









    static void Main()
    {
        //1
        Thread thread = new Thread(PrintNumbers);
        thread.Start();
        
        
        
        //2
        Console.Write("Початок діапазону: ");
        int start = int.Parse(Console.ReadLine());

        Console.Write("Кінець діапазону: ");
        int end = int.Parse(Console.ReadLine());

        Thread thread1 = new Thread(() => PrintNumbers(start, end));
        thread.Start();



        //3
        Console.Write("Початок діапазону: ");
        int start1 = int.Parse(Console.ReadLine());

        Console.Write("Кінець діапазону: ");
        int end1 = int.Parse(Console.ReadLine());

        Console.Write("Кількість потоків: ");
        int threadCount = int.Parse(Console.ReadLine());

        int range = (end1 - start1 + 1) / threadCount;
        int currentStart = start1;

        for (int i = 0; i < threadCount; i++)
        {
            int localStart = currentStart;
            int localEnd = (i == threadCount - 1) ? end1 : currentStart + range - 1;
            Thread t = new Thread(() => PrintNumbers(localStart, localEnd));
            t.Start();
            currentStart += range;
        }



        //4

        Random rnd = new Random();
        for (int i = 0; i < numbers.Length; i++)
            numbers[i] = rnd.Next(1, 10001);

        Thread t1 = new Thread(FindMin);
        Thread t2 = new Thread(FindMax);
        Thread t3 = new Thread(FindAverage);

        t1.Start();
        t2.Start();
        t3.Start();

        t1.Join();
        t2.Join();
        t3.Join();

        Console.WriteLine($"Min: {min}");
        Console.WriteLine($"Max: {max}");
        Console.WriteLine($"Average: {avg:F2}");



        //5
        rnd = new Random();
        for (int i = 0; i < numbers.Length; i++)
            numbers[i] = rnd.Next(1, 10001);

        t1 = new Thread(FindMin);
        t2 = new Thread(FindMax);
        t3 = new Thread(FindAverage);

        t1.Start(); t2.Start(); t3.Start();
        t1.Join(); t2.Join(); t3.Join();

        Thread t4 = new Thread(WriteToFile);
        t4.Start();
        t4.Join();

        Console.WriteLine("Дані записано у файл results.txt");

    }





}
