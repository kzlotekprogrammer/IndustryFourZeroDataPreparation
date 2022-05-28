using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndustryFourZeroDataPreparation
{
    internal class DataManager
    {
        List<WeatherSample> weatherSamples;
        List<MeasurmentSample> measurementSamples;

        SampleLoader dataLoader = new SampleLoader();
        OutputFormatter outputFormatter = new OutputFormatter();

        public void PrepareData(string weatherSamplesDirPath, string measurmentSamplesDirPath)
        {
            weatherSamples = dataLoader.LoadWeatherSamples(weatherSamplesDirPath);
            measurementSamples = dataLoader.LoadMeasurmentSamples(measurmentSamplesDirPath);
            outputFormatter.FormatAndSave(weatherSamples, measurementSamples);
        }
    }
}
