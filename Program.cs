using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static SemaphoreSlim sem = new SemaphoreSlim(3, 3);
    static Random rnd = new Random();

    static async Task Main()
    {
        Console.WriteLine("Начало");
        Console.WriteLine("---");

        Task[] mas = new Task[7];
        int count = 0;

        for (int i = 0; i < 7; i++)
        {
            int n = i + 1;
            mas[i] = Task.Run(() => Work(n));
            count++;
        }

        await Task.WhenAll(mas);

        Console.WriteLine("---");
        Console.WriteLine("Конец");
    }

    static async Task Work(int n)
    {
        Console.WriteLine("Студент " + n + " ждет");

        await sem.WaitAsync();

        Console.WriteLine("Студент " + n + " сел за комп, свободно: " + sem.CurrentCount);

        int t = rnd.Next(1000, 3000);
        await Task.Delay(t);

        Console.WriteLine("Студент " + n + " закончил");
        sem.Release();
    }
}
