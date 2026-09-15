namespace SmartHome.Common
{
    public class SmartLight : Device
    {
        public int Brightness { get; set; }
        public string ColorHex { get; set; }
        public bool SupportsRGB { get; set; }

        public SmartLight() : base() { }
        private static readonly Random _random = new Random();

        public static SmartLight CreateNew()
        {
            return new SmartLight(
                $"Light_{_random.Next(1000, 9999)}",
                _random.Next(0, 101),
                "#FFFFFF",
                true
            );
        }

        public SmartLight(string name, int brightness, string colorHex, bool supportsRGB)
            : base(name)
        {
            Brightness = brightness;
            ColorHex = colorHex;
            SupportsRGB = supportsRGB;
        }

        public override string ToString()
        {
            return $"[SmartLight] ID: {Id} | Назва: {Name} | Стан: {(IsOn ? "УВІМК" : "ВИМК")} | Яскравість: {Brightness}% | Колір: {ColorHex}";
        }
    }
}