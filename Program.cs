using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static SemaphoreSlim computers = new SemaphoreSlim(3, 3);
    static Random random = new Random();

    static async Task Main()
    {
        Task[] students = new Task[7];
        
        for (int i = 0; i < students.Length; i++)
        {
            int studentId = i + 1;
            students[i] = Task.Run(() => DoTask(studentId));
        }

        await Task.WhenAll(students);
        
        Console.WriteLine("\nВсе студенты выполнили задания.");
    }

    static async Task DoTask(int studentId)
    {
        Console.WriteLine($"Студент {studentId} ждёт компьютер...");

        await computers.WaitAsync();

        try
        {
            Console.WriteLine($"--> Студент {studentId} сел за компьютер. " +
                              $"Свободно: {computers.CurrentCount}");
            
            await Task.Delay(random.Next(1000, 3000));
            
            Console.WriteLine($"<-- Студент {studentId} закончил работу.");
        }
        finally
        {
            computers.Release();
        }
    }
}
