using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IndustryFourZeroDataPreparation
{
    internal class OutputFormatter
    {
        public void FormatAndSave(List<WeatherSample> weatherSamples, List<MeasurmentSample> measurmentSamples)
        {
            List<OutputSample> outputSamples = FormatSamples(weatherSamples, measurmentSamples);
            SaveOutputSamples(outputSamples);
        }

        private void SaveOutputSamples(List<OutputSample> outputSamples)
        {
            using (StreamWriter streamWriter = new StreamWriter("TrainingDataWithDate.txt"))
            {
                streamWriter.WriteLine(";Wejście:");
                streamWriter.WriteLine(";   Pyranometr [W/m2]");
                streamWriter.WriteLine(";   Temperatura [°C]");
                streamWriter.WriteLine(";   Temperatura odczuwalna [°C]");
                streamWriter.WriteLine(";   Kierunek wiatru [°]");
                streamWriter.WriteLine(";   Prędkość wiatru [Km/h]");
                streamWriter.WriteLine(";   Wilgotność względna");
                streamWriter.WriteLine(";   Punkt rosy [°C]");
                streamWriter.WriteLine(";   Ciśnienie [mb]");
                streamWriter.WriteLine(";   Rodzaj pogody:");
                streamWriter.WriteLine(";       Bezchmurnie=0");
                streamWriter.WriteLine(";       Niewielkie zachmurzenie=1");
                streamWriter.WriteLine(";       Częściowe zachmurzenie=2");
                streamWriter.WriteLine(";       Pochmurno=3");
                streamWriter.WriteLine(";       Marznąca mgła=4");
                streamWriter.WriteLine(";       Deszcz=5");
                streamWriter.WriteLine(";       Burza=6");
                streamWriter.WriteLine(";       Śnieg=7");
                streamWriter.WriteLine(";Wyjście:");
                streamWriter.WriteLine(";   E-AC_(sc-Si)_[Wh]");

                foreach (OutputSample outputSample in outputSamples)
                {
                    WeatherSample weatherSample = outputSample.Weather;
                    MeasurmentSample measurmentSample = outputSample.Measurment;
                    streamWriter.WriteLine($"{weatherSample.Date}\t{measurmentSample.Pyranometr}\t{weatherSample.Temperature}\t{weatherSample.TemperatureFelt}\t" +
                        $"{weatherSample.WindDirection}\t{weatherSample.WindSpeed}\t{weatherSample.RelativeHumidity}\t{weatherSample.DewPoint}" +
                        $"\t{weatherSample.Pressure}\t{weatherSample.WeatherKind}\t{measurmentSample.AlternatingCurrent}");
                }
            }
        }

        private List<OutputSample> FormatSamples(List<WeatherSample> weatherSamples, List<MeasurmentSample> measurmentSamples)
        {
            List<OutputSample> outputSamples = new List<OutputSample>();

            weatherSamples.Sort((w1, w2) => w1.Date.CompareTo(w2.Date));
            measurmentSamples.Sort((m1, m2) => m1.Date.CompareTo(m2.Date));
            int index = 0;

            foreach (WeatherSample weatherSample in weatherSamples)
            {
                OutputSample outSamp = new OutputSample();
                outSamp.Weather = weatherSample;

                DateTime startTime = weatherSample.Date;
                DateTime endTime = weatherSample.Date.AddMinutes(30);

                List<MeasurmentSample> measSamplToAgg = new List<MeasurmentSample>();
                for (; index < measurmentSamples.Count; index++)
                {
                    if (measurmentSamples[index].Date < startTime)
                        continue;
                    else if (measurmentSamples[index].Date < endTime)
                        measSamplToAgg.Add(measurmentSamples[index]);
                    else
                        break;
                }

                if (measSamplToAgg.Count == 0)
                    continue;

                outSamp.Measurment = new MeasurmentSample()
                {
                    Date = startTime,
                    Pyranometr = measSamplToAgg.Sum(m => m.Pyranometr) / measSamplToAgg.Count,
                    AlternatingCurrent = measSamplToAgg.Sum((m) => m.AlternatingCurrent) / measSamplToAgg.Count,
                };
                outputSamples.Add(outSamp);
            }

            return outputSamples;
        }
    }
}
