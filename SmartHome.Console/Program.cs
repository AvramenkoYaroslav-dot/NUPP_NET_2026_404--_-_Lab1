using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Common;

namespace SmartHome.ConsoleApp
{
    internal class Program
    {
        private static readonly object _lockObject = new object();
        private static readonly AutoResetEvent _autoEvent = new AutoResetEvent(false);

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №2: Багатопотоковість, Асинхронність, LINQ ===\n");

            string filePath = "devices_lab2.json";
            var service = new InMemoryCrudServiceAsync<SmartLight>(filePath);

            // 1. Паралельне створення >1000 об'єктів з використанням Parallel.For
            Console.WriteLine("1. Паралельне створення 1200 об'єктів SmartLight...");
            Parallel.For(0, 1200, i =>
            {
                var light = SmartLight.CreateNew();
                service.CreateAsync(light).GetAwaiter().GetResult();
            });

            var allDevices = (await service.ReadAllAsync()).ToList();
            Console.WriteLine($"Успішно створено пристроїв: {allDevices.Count}");

            // 2. Використання LINQ для пошуку Min, Max, Average
            Console.WriteLine("\n2. Статистичний аналіз цифрових значень (LINQ):");
            int minBrightness = allDevices.Min(d => d.Brightness);
            int maxBrightness = allDevices.Max(d => d.Brightness);
            double avgBrightness = allDevices.Average(d => d.Brightness);

            Console.WriteLine($" - Мінімальна яскравість: {minBrightness}%");
            Console.WriteLine($" - Максимальна яскравість: {maxBrightness}%");
            Console.WriteLine($" - Середня яскравість:   {avgBrightness:F2}%");

            // 3. Пагінація
            Console.WriteLine("\n3. Перевірка пагінації (Сторінка 1, 5 елементів):");
            var paged = await service.ReadAllAsync(page: 1, amount: 5);
            foreach (var dev in paged)
            {
                Console.WriteLine($"   {dev}");
            }

            // 4. Демонстрація примітивів синхронізації
            Console.WriteLine("\n4. Демонстрація примітивів синхронізації:");
            DemonstrateSyncPrimitives();

            // 5. Асинхронне збереження у файл
            Console.WriteLine("\n5. Збереження колекції у файл...");
            bool saved = await service.SaveAsync();
            Console.WriteLine(saved ? $"Файл успішно збережено за шляхом: {Path.GetFullPath(filePath)}" : "Помилка збереження.");

            Console.WriteLine("\nРоботу програми завершено успішно.");
        }

        private static void DemonstrateSyncPrimitives()
        {
            // Lock
            int counter = 0;
            Parallel.For(0, 100, i =>
            {
                lock (_lockObject)
                {
                    counter++;
                }
            });
            Console.WriteLine($" - [Lock] Лічильник після 100 паралельних інкрементів: {counter}");

            // SemaphoreSlim
            using (var semaphore = new SemaphoreSlim(2, 2))
            {
                int activeAccess = 0;
                Parallel.For(0, 5, i =>
                {
                    semaphore.Wait();
                    Interlocked.Increment(ref activeAccess);
                    Thread.Sleep(20);
                    Interlocked.Decrement(ref activeAccess);
                    semaphore.Release();
                });
                Console.WriteLine(" - [SemaphoreSlim] Демонстрація обмеження одночасного доступу виконана.");
            }

            // AutoResetEvent
            Task.Run(() =>
            {
                Thread.Sleep(50);
                _autoEvent.Set();
            });
            _autoEvent.WaitOne();
            Console.WriteLine(" - [AutoResetEvent] Сигнал отримано від фонового потоку.");
        }
    }
}