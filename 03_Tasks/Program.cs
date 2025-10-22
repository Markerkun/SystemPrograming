using System;
using System.Threading.Tasks;

class Program
{
    //1
    static void ShowTime(string title)
    {
        Console.WriteLine($"{title}: {DateTime.Now}");
    }

    //2
    static void PrintPrimes(int start, int end)
    {
        for (int i = start; i <= end; i++)
            if (IsPrime(i))
                Console.Write(i + " ");
    }

    static bool IsPrime(int n)
    {
        if (n < 2) return false;
        for (int i = 2; i * i <= n; i++)
            if (n % i == 0) return false;
        return true;
    }

    //3
    static int CountPrimes(int start, int end)
    {
        int count = 0;
        for (int i = start; i <= end; i++)
            if (IsPrime(i))
                count++;
        return count;
    }


    static void Main()
    {
        //1

        Task task = new Task(() => ShowTime("Task 1 (Start)"));
        task.Start();


        Task task1 = Task.Factory.StartNew(() => ShowTime("Task 2 (Factory.StartNew)"));


        Task task2 = Task.Run(() => ShowTime("Task 3 (Run)"));

        Task.WaitAll(task, task1, task2 );
        Console.WriteLine("Completed");

        //2
        Task task3 = Task.Run(() => PrintPrimes(0, 1000));
        task3.Wait();
        Console.WriteLine("Completed");

        //3
        Console.Write("Start: ");
        int start = int.Parse(Console.ReadLine());

        Console.Write("End: ");
        int end = int.Parse(Console.ReadLine());

        Task<int> task4 = Task.Run(() => CountPrimes(start, end));


        int count = task4.Result;

        Console.WriteLine($"Amount of prime nums in [{start}; {end}] = {count}");

        //4
        int[] numbers = new int[100];
        for (int i = 0; i < numbers.Length; i++)
            numbers[i] = i + 1;

        Task<int> tMin = Task.Run(() => numbers.Min());
        Task<int> tMax = Task.Run(() => numbers.Max());
        Task<double> tAvg = Task.Run(() => numbers.Average());
        Task<int> tSum = Task.Run(() => numbers.Sum());

        Task.WaitAll(tMin, tMax, tAvg, tSum);

        Console.WriteLine($"Min: {tMin.Result}");
        Console.WriteLine($"Max: {tMax.Result}");
        Console.WriteLine($"Average: {tAvg.Result}");
        Console.WriteLine($"Sum: {tSum.Result}");

        //5
        int[] nums = { 5, 3, 8, 3, 2, 8, 1, 5, 9, 2, 4, 4 };

        Task<int[]> taska1 = Task.Run(() =>
        {
            Console.WriteLine("Deleting repeting values");
            return nums.Distinct().ToArray();
        });

        Task<int[]> taska2 = taska1.ContinueWith(prev =>
        {
            Console.WriteLine("Sorting");
            var sorted = prev.Result.OrderBy(x => x).ToArray();
            return sorted;
        });

        Task taska3 = taska2.ContinueWith(prev =>
        {
            Console.WriteLine("What number I should find: ");
            int value = int.Parse(Console.ReadLine());

            int index = Array.BinarySearch(prev.Result, value);
            if (index >= 0)
                Console.WriteLine($"The number {value} was found at {index} index");
            else
                Console.WriteLine($"The number {value} wasn't found");
        });

        task3.Wait();
        Console.WriteLine("Completed");
    }

    }


}
