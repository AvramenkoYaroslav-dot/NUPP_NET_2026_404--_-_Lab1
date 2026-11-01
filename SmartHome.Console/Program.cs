using Microsoft.EntityFrameworkCore;
using SmartHome.Common;
using SmartHome.Infrastructure;
using SmartHome.Infrastructure.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Лабораторна робота №3: Entity Framework Core & SQLite ===\n");

            using (var context = new SmartHomeContext())
            {
                // Автоматичне застосування міграцій та створення БД
                await context.Database.MigrateAsync();

                var repository = new Repository<DeviceModel>(context);
                var service = new DbCrudServiceAsync(repository);

                // 1. Створення кімнати та 1-до-1 датчика
                Console.WriteLine("1. Створення Кімнати та Датчика Клімату (1-до-1)...");
                var room = new RoomModel { Name = "Вітальня" };
                var sensor = new ClimateSensorModel { SerialNumber = "SENS-101", Humidity = 45.5, Room = room };
                context.Rooms.Add(room);
                context.ClimateSensors.Add(sensor);
                await context.SaveChangesAsync();

                // 2. Створення пристроїв TPT (1-до-багатьох з Кімнатою)
                Console.WriteLine("\n2. Додавання пристроїв (TPT Таблиця-на-тип) до БД...");
                var light = new SmartLightModel { Name = "Люстра", Brightness = 85, Color = "#FFFFFF", RoomId = room.Id };
                var thermo = new ThermostatModel { Name = "Термостат", TargetTemperature = 22.5, CurrentTemperature = 21.0, Mode = "Auto", RoomId = room.Id };

                await service.CreateAsync(light);
                await service.CreateAsync(thermo);

                // 3. Зчитування з бази даних через сервіс
                Console.WriteLine("\n3. Читання пристроїв із SQLite БД через Репозиторій:");
                var devices = await service.ReadAllAsync();
                foreach (var d in devices)
                {
                    Console.WriteLine($" - [ID: {d.Id}] Name: {d.Name}, RoomId: {d.RoomId}");
                }

                // 4. Демонстрація Багато-до-багатьох (Scenario & Devices)
                Console.WriteLine("\n4. Створення Сценарію та додавання пристроїв (Багато-до-багатьох)...");
                var scenario = new ScenarioModel { Title = "Вечірній режим" };
                context.Scenarios.Add(scenario);
                await context.SaveChangesAsync();

                context.ScenarioDevices.Add(new ScenarioDeviceModel { ScenarioId = scenario.Id, DeviceId = light.Id });
                context.ScenarioDevices.Add(new ScenarioDeviceModel { ScenarioId = scenario.Id, DeviceId = thermo.Id });
                await context.SaveChangesAsync();

                Console.WriteLine($"Сценарій '{scenario.Title}' успішно зв'язано з {context.ScenarioDevices.Count()} пристроями.");
            }

            Console.WriteLine("\nЛабораторну роботу №3 успішно виконано.");
        }
    }
}