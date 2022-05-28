using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustryFourZeroDataPreparation
{
    internal class Program
    {
        private static string WeatherSamplesDirPath = @"C:\Users\kzlot\Documents\Studia\10 semestr\Przemysl4\Pogoda";
        private static string MeasurmentSamplesDirPath = @"C:\Users\kzlot\Documents\Studia\10 semestr\Przemysl4\Pyranometr";

        static void Main(string[] args)
        {
            DataManager dataManager = new DataManager();
            dataManager.PrepareData(WeatherSamplesDirPath, MeasurmentSamplesDirPath);
        }
    }
}
