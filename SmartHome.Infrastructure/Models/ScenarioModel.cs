using System.Collections.Generic;

namespace SmartHome.Infrastructure.Models
{
    public class ScenarioModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<ScenarioDeviceModel> ScenarioDevices { get; set; } = new List<ScenarioDeviceModel>();
    }
}