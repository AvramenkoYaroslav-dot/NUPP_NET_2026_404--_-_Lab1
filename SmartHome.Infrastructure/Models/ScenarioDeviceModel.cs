using System;

namespace SmartHome.Infrastructure.Models
{
    public class ScenarioDeviceModel
    {
        public int ScenarioId { get; set; }
        public ScenarioModel Scenario { get; set; }

        public Guid DeviceId { get; set; }
        public DeviceModel Device { get; set; }
    }
}