using System;

namespace SmartHome.Common
{
    // Делегат для події
    public delegate void DeviceStatusChangedHandler(string deviceName, bool isON);

    public abstract class Device
    {
        // Статичне поле
        public static int TotalDevicesCount;

        // Подія
        public event DeviceStatusChangedHandler StatusChanged;

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool IsOn { get; protected set; }
        public DateTime InstallationDate { get; set; }

        // Статичний конструктор
        static Device()
        {
            TotalDevicesCount = 0;
        }

        // Конструктор за замовчуванням
        public Device()
        {
            TotalDevicesCount++;
            InstallationDate = DateTime.Now;
        }

        public Device(string name) : this()
        {
            Name = name;
        }

        // Віртуальні методи
        public virtual void TurnOn()
        {
            IsOn = true;
            StatusChanged?.Invoke(Name, IsOn);
        }

        public virtual void TurnOff()
        {
            IsOn = false;
            StatusChanged?.Invoke(Name, IsOn);
        }

        // Статичний метод
        public static void DisplayTotalDevices()
        {
            Console.WriteLine($"[INFO] Всього створено пристроїв: {TotalDevicesCount}");
        }
    }
}