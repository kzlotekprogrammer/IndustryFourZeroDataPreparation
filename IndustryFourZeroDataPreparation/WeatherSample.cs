using System;

namespace IndustryFourZeroDataPreparation
{
    internal class WeatherSample
    {
        public DateTime Date { get; set; }
        public int Temperature { get; set; }
        public int TemperatureFelt { get; set; }
        public int WindDirection { get; set; }
        public int WindSpeed { get; set; }
        public double RelativeHumidity { get; set; }
        public int DewPoint { get; set; }
        public int Pressure { get; set; }
        public int WeatherKind { get; set; }
    }
}
