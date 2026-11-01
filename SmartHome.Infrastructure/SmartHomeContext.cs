using Microsoft.EntityFrameworkCore;
using SmartHome.Infrastructure.Models;

namespace SmartHome.Infrastructure
{
    public class SmartHomeContext : DbContext
    {
        public DbSet<DeviceModel> Devices { get; set; }
        public DbSet<SmartLightModel> SmartLights { get; set; }
        public DbSet<ThermostatModel> Thermostats { get; set; }
        public DbSet<RoomModel> Rooms { get; set; }
        public DbSet<ClimateSensorModel> ClimateSensors { get; set; }
        public DbSet<ScenarioModel> Scenarios { get; set; }
        public DbSet<ScenarioDeviceModel> ScenarioDevices { get; set; }

        public SmartHomeContext()
        {
        }

        public SmartHomeContext(DbContextOptions<SmartHomeContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=smarthome_lab3.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. TPT (Table-per-Type) Наслідування для Пристроїв
            modelBuilder.Entity<DeviceModel>().ToTable("Devices");
            modelBuilder.Entity<SmartLightModel>().ToTable("SmartLights");
            modelBuilder.Entity<ThermostatModel>().ToTable("Thermostats");

            // 2. Зв'язок 1-до-багатьох (Room 1 -> N Devices)
            modelBuilder.Entity<RoomModel>()
                .HasMany(r => r.Devices)
                .WithOne(d => d.Room)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.SetNull);

            // 3. Зв'язок 1-до-1 (Room 1 <-> 1 ClimateSensor)
            modelBuilder.Entity<RoomModel>()
                .HasOne(r => r.ClimateSensor)
                .WithOne(cs => cs.Room)
                .HasForeignKey<ClimateSensorModel>(cs => cs.RoomId);

            // 4. Зв'язок Багато-до-багатьох (Many-to-Many: Device <-> Scenario)
            modelBuilder.Entity<ScenarioDeviceModel>()
                .HasKey(sd => new { sd.ScenarioId, sd.DeviceId });

            modelBuilder.Entity<ScenarioDeviceModel>()
                .HasOne(sd => sd.Scenario)
                .WithMany(s => s.ScenarioDevices)
                .HasForeignKey(sd => sd.ScenarioId);

            modelBuilder.Entity<ScenarioDeviceModel>()
                .HasOne(sd => sd.Device)
                .WithMany(d => d.ScenarioDevices)
                .HasForeignKey(sd => sd.DeviceId);
        }
    }
}