namespace SmartHome.Common
{
    public class Thermostat : Device
    {
        public double TargetTemperature { get; set; }
        public double CurrentTemperature { get; set; }
        public string Mode { get; set; }

        public Thermostat() : base() { }

        public Thermostat(string name, double targetTemp, double currentTemp, string mode)
            : base(name)
        {
            TargetTemperature = targetTemp;
            CurrentTemperature = currentTemp;
            Mode = mode;
        }

        public override string ToString()
        {
            return $"[Thermostat] ID: {Id} | Назва: {Name} | Стан: {(IsOn ? "УВІМК" : "ВИМК")} | Темп: {CurrentTemperature}°C -> Ціль: {TargetTemperature}°C";
        }
    }
}