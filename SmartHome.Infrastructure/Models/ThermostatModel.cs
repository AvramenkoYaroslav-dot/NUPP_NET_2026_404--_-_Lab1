namespace SmartHome.Infrastructure.Models
{
    public class ThermostatModel : DeviceModel
    {
        public double TargetTemperature { get; set; }
        public double CurrentTemperature { get; set; }
        public string Mode { get; set; }
    }
}