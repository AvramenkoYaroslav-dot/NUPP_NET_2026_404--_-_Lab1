namespace SmartHome.Infrastructure.Models
{
    public class ClimateSensorModel
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public double Humidity { get; set; }

        // Зв'язок 1-до-1 з Кімнатою
        public int RoomId { get; set; }
        public RoomModel Room { get; set; }
    }
}