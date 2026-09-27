using System.Collections.Generic;

namespace SmartHome.Infrastructure.Models
{
    public class RoomModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Зв'язок 1-до-багатьох: У кімнаті багато Пристроїв
        public ICollection<DeviceModel> Devices { get; set; } = new List<DeviceModel>();

        // Зв'язок 1-до-1: У кімнати є один Датчик Клімату
        public ClimateSensorModel ClimateSensor { get; set; }
    }
}