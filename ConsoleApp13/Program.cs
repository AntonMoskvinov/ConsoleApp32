using System;
using System.Threading;

class Program
{
    static Mutex mutex = new Mutex();
    static int[] data = { 1, 2, 3, 4, 5 };
    static bool modificationDone = false;

    static void Main()
    {
        Thread firstThread = new Thread(ModifyArray);
        Thread secondThread = new Thread(FindMaxValue);

        firstThread.Start();
        secondThread.Start();

        firstThread.Join();
        secondThread.Join();

        Console.WriteLine("Modified Array:");
        foreach (var element in data)
        {
            Console.Write($"{element} ");
        }

        Console.WriteLine("\nMaximum Value in the Array: " + FindMaxValueInArray());
    }

    static void ModifyArray()
    {
        mutex.WaitOne();

        try
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] += 10; 
                Thread.Sleep(200); 
            }

            modificationDone = true;
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    static void FindMaxValue()
    {
        while (!modificationDone)
        {
            
            Thread.Sleep(100);
        }

        mutex.WaitOne();

        try
        {
            int maxValue = FindMaxValueInArray();
            Console.WriteLine($"Thread 2: Maximum Value in the Array: {maxValue}");
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    static int FindMaxValueInArray()
    {
        int maxValue = int.MinValue;

        foreach (var element in data)
        {
            if (element > maxValue)
            {
                maxValue = element;
            }
        }

        return maxValue;
    }
}
