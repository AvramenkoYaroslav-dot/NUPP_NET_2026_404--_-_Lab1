using System;
using System.Linq;
using SmartHome.Common;

namespace SmartHome.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=================================================");
            Console.WriteLine("   ЛАБОРАТОРНА РОБОТА №1 - СИСТЕМА SMART HOME   ");
            Console.WriteLine("=================================================\n");

            // 1. Статичний метод
            Device.DisplayTotalDevices();

            // 2. Створення CRUD сервісу
            ICrudService<SmartLight> lightService = new InMemoryCrudService<SmartLight>();

            var light1 = new SmartLight("Люстра у вітальні", 100, "#FFFFFF", true);
            var light2 = new SmartLight("Нічна лампа", 30, "#FF8C00", false);

            // Підписка на подію (Делегат)
            light1.StatusChanged += OnDeviceStatusChanged;
            light2.StatusChanged += OnDeviceStatusChanged;

            // 3. CREATE
            Console.WriteLine("\n[1] Додавання об'єктів (CREATE)...");
            lightService.Create(light1);
            lightService.Create(light2);

            // READ ALL
            Console.WriteLine("\nСписок об'єктів у сервісі (READ ALL):");
            foreach (var light in lightService.ReadAll())
            {
                Console.WriteLine(light);
            }

            // 4. Демонстрація методів розширення та подій
            Console.WriteLine("\n[2] Події та метод розширення...");
            light1.TurnOn(); // Викликає подію
            Console.WriteLine(light1.GetStatusReport()); // Метод розширення

            // 5. UPDATE
            Console.WriteLine("\n[3] Оновлення об'єкта (UPDATE)...");
            light1.Brightness = 50;
            light1.ColorHex = "#00FF00";
            lightService.Update(light1);
            Console.WriteLine($"Оновлений об'єкт з базі: {lightService.Read(light1.Id)}");

            // 6. ДОДАТКОВЕ ЗАВДАННЯ (Save & Load)
            string filePath = "smart_lights.json";
            Console.WriteLine($"\n[4] Збереження у JSON файл ({filePath})...");
            lightService.Save(filePath);
            Console.WriteLine("Успішно збережено!");

            Console.WriteLine("\nЗавантаження у новий екземпляр сервісу з файлу...");
            ICrudService<SmartLight> newLightService = new InMemoryCrudService<SmartLight>();
            newLightService.Load(filePath);

            foreach (var light in newLightService.ReadAll())
            {
                Console.WriteLine($"[З файлу] {light}");
            }

            // 7. REMOVE
            Console.WriteLine("\n[5] Видалення об'єкта (REMOVE)...");
            newLightService.Remove(light2);
            Console.WriteLine($"Кількість елементів після видалення: {newLightService.ReadAll().Count()}");

            Console.WriteLine("\n=================================================");
            Device.DisplayTotalDevices();

            Console.WriteLine("\nНатисніть будь-яку клавішу для завершення...");
            Console.ReadKey();
        }

        private static void OnDeviceStatusChanged(string deviceName, bool isON)
        {
            Console.WriteLine($"  [ПОДІЯ] Пристрій '{deviceName}' змінив стан на: {(isON ? "УВІМКНЕНО" : "ВИМКНЕНО")}");
        }
    }
}