using System;

namespace SmartHome.Common
{
    public class Room
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RoomName { get; set; }
        public double AreaSqMeters { get; set; }
        public int Floor { get; set; }

        public Room() { }

        public Room(string roomName, double areaSqMeters, int floor)
        {
            RoomName = roomName;
            AreaSqMeters = areaSqMeters;
            Floor = floor;
        }

        public override string ToString()
        {
            return $"[Room] ID: {Id} | Назва: {RoomName} | Площа: {AreaSqMeters} m² | Поверх: {Floor}";
        }
    }
}