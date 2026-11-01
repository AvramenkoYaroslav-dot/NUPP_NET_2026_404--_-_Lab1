using System;
using System.Collections.Generic;

namespace SmartHome.Infrastructure.Models
{
    public abstract class DeviceModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public bool IsOn { get; set; }

        // Зв'язок 1-до-багатьох: Пристрій належить одній Кімнаті
        public int? RoomId { get; set; }
        public RoomModel Room { get; set; }

        // Зв'язок Багато-до-багатьох: Пристрій може належати багатьом Сценаріям (SmartScenarios)
        public ICollection<ScenarioDeviceModel> ScenarioDevices { get; set; } = new List<ScenarioDeviceModel>();
    }
}