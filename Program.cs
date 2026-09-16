using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    // Семафор: 3 разрешения = 3 компьютера
    static SemaphoreSlim computers = new SemaphoreSlim(3, 3);
    static Random random = new Random();

    static async Task Main()
    {
        // Создаём 7 "студентов" (задач)
        Task[] students = new Task[7];
        
        for (int i = 0; i < students.Length; i++)
        {
            int studentId = i + 1;
            students[i] = Task.Run(() => DoTask(studentId));
        }

        // Ждём завершения задач
        await Task.WhenAll(students);
        
        Console.WriteLine("\nВсе студенты выполнили задания.");
    }

    static async Task DoTask(int studentId)
    {
        Console.WriteLine($"Студент {studentId} ждёт компьютер...");

        // Пытаемся занять разрешение (компьютер)
        await computers.WaitAsync();

        try
        {
            Console.WriteLine($"--> Студент {studentId} сел за компьютер. " +
                              $"Свободно: {computers.CurrentCount}");
            
            // Имитация работы за пк (1-3 секунды)
            await Task.Delay(random.Next(1000, 3000));
            
            Console.WriteLine($"<-- Студент {studentId} закончил работу.");
        }
        finally
        {
            // освободить семафор в finally
            computers.Release();
        }
    }
}
