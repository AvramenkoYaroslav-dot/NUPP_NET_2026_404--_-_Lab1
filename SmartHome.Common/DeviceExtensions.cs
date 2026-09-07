namespace SmartHome.Common
{
    public static class DeviceExtensions
    {
        // Метод розширення
        public static string GetStatusReport(this Device device)
        {
            return $"[EXTENSION REPORT] Пристрій '{device.Name}' наразі {(device.IsOn ? "активний" : "неактивний")}.";
        }
    }
}