using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static SemaphoreSlim sem = new SemaphoreSlim(3, 3);
    static Random rand = new Random();

    static async Task Main()
    {
        Console.WriteLine("Студенты идут к компьютерам");
        Console.WriteLine();

        Task[] mas = new Task[7];

        for (int i = 0; i < 7; i++)
        {
            int n = i + 1;
            mas[i] = Task.Run(() => Work(n));
        }

        await Task.WhenAll(mas);
        Console.WriteLine();
        Console.WriteLine("Все закончили");
    }

    static async Task Work(int n)
    {
        Console.WriteLine("Студент " + n + " ждет...");

        await sem.WaitAsync();

        try
        {
            Console.WriteLine("Студент " + n + " сел. Свободно: " + sem.CurrentCount);
            
            int t = rand.Next(1000, 3000);
            await Task.Delay(t);
            
            Console.WriteLine("Студент " + n + " освободил компьютер");
        }
        finally
        {
            sem.Release();
        }
    }
}
