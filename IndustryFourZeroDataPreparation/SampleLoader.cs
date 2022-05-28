using System;
using System.Collections.Generic;
using System.IO;

namespace IndustryFourZeroDataPreparation
{
    internal class SampleLoader
    {
        public List<WeatherSample> LoadWeatherSamples(string directoryPath)
        {
            List<WeatherSample> weatherSamples = new List<WeatherSample>();

            foreach (string filePath in Directory.GetFiles(directoryPath))
            {
                weatherSamples.AddRange(LoadWeatherSamplesFromFile(filePath));
            }

            return weatherSamples;
            //weatherData.Select(x => x.Description).Distinct().ToList().ForEach(x => Console.WriteLine(x));
        }

        private List<WeatherSample> LoadWeatherSamplesFromFile(string filePath)
        {
            List<WeatherSample> weatherDaySamples = new List<WeatherSample>();

            int skipLine = 1;
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (reader.Peek() >= 0)
                {
                    string[] weatherSampleValues = ReadLineValuesUtf16LE(reader, 10);
                    if (skipLine > 0)
                    {
                        skipLine--;
                        continue;
                    }

                    weatherDaySamples.Add(ParseWeatherSampleValues(weatherSampleValues));
                }
            }

            return weatherDaySamples;
        }

        private string[] ReadLineValuesUtf16LE(StreamReader reader, int valuesRequied)
        {
            string line = "";
            while (reader.Peek() >= 0)
            {
                line += reader.ReadLine();
                string[] lineValues = line.Split(new char[] { '\t' });
                if (lineValues.Length == valuesRequied)
                    return lineValues;
                else if (lineValues.Length > valuesRequied)
                    throw new Exception("Invalid data sample: " + line);
            }

            throw new Exception("Incomplete data sample: " + line);
        }

        private WeatherSample ParseWeatherSampleValues(string[] weatherSampleValues)
        {
            WeatherSample weatherSample = new WeatherSample();

            weatherSample.Date = DateTime.Parse($"{weatherSampleValues[0]} {weatherSampleValues[1]}");

            weatherSampleValues[2] = weatherSampleValues[2].Replace("°C", "");
            weatherSample.Temperature = int.Parse(weatherSampleValues[2]);

            weatherSampleValues[3] = weatherSampleValues[3].Replace("°C", "");
            weatherSample.TemperatureFelt = int.Parse(weatherSampleValues[3]);

            weatherSampleValues[4] = weatherSampleValues[4].Replace("Zmienne na wysokości ", "").Replace(" Km/h", "").Replace("\"", "");
            string[] windValues = weatherSampleValues[4].Split(new char[] { '°' });
            if (windValues.Length > 1)
            {
                weatherSample.WindDirection = int.Parse(windValues[0]);
                weatherSample.WindSpeed = int.Parse(windValues[1]);
            }
            else
            {
                weatherSample.WindDirection = 0;
                if (int.TryParse(windValues[0], out int weatherSpeed))
                    weatherSample.WindSpeed = weatherSpeed;
            }

            weatherSample.RelativeHumidity = double.Parse(weatherSampleValues[6]);

            weatherSampleValues[7] = weatherSampleValues[7].Replace("°C", "");
            weatherSample.DewPoint = int.Parse(weatherSampleValues[7]);

            weatherSampleValues[8] = weatherSampleValues[8].Replace("mb", "").Replace("\"", "").Replace(",0", "");
            weatherSample.Pressure = int.Parse(weatherSampleValues[8]);

            if (weatherSampleValues[9].Contains("Śnieg"))
                weatherSample.WeatherKind = 7;
            else if (weatherSampleValues[9].Contains("Burza"))
                weatherSample.WeatherKind = 6;
            else if (weatherSampleValues[9].Contains("Deszcz"))
                weatherSample.WeatherKind = 5;
            else if (weatherSampleValues[9].Contains("Marznąca mgła"))
                weatherSample.WeatherKind = 4;
            else if (weatherSampleValues[9].Contains("Pochmurno"))
                weatherSample.WeatherKind = 3;
            else if (weatherSampleValues[9].Contains("Częściowe zachmurzenie"))
                weatherSample.WeatherKind = 2;
            else if (weatherSampleValues[9].Contains("Niewielkie zachmurzenie"))
                weatherSample.WeatherKind = 1;
            else if (weatherSampleValues[9].Contains("Bezchmurnie"))
                weatherSample.WeatherKind = 0;
            else
                throw new Exception("Unsupported description: " + weatherSampleValues[9]);

            return weatherSample;
        }

        public List<MeasurmentSample> LoadMeasurmentSamples(string directoryPath)
        {
            List<MeasurmentSample> measurmentSamples = new List<MeasurmentSample>();

            foreach (string filePath in Directory.GetFiles(directoryPath))
            {
                measurmentSamples.AddRange(LoadMeasurmentSamplesFromFile(filePath));
            }

            return measurmentSamples;
        }

        private List<MeasurmentSample> LoadMeasurmentSamplesFromFile(string filePath)
        {
            List<MeasurmentSample> measurmentSamples = new List<MeasurmentSample>();

            int skipLine = 3;
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (reader.Peek() >= 0)
                {
                    string[] measureSampleValues = reader.ReadLine().Split(new char[] { '\t' });
                    if (skipLine > 0)
                    {
                        skipLine--;
                        continue;
                    }

                    measurmentSamples.Add(ParseMeasurmentSampleValues(measureSampleValues));
                }
            }

            return measurmentSamples;
        }

        private MeasurmentSample ParseMeasurmentSampleValues(string[] measureSampleValues)
        {
            MeasurmentSample measurmentSample = new MeasurmentSample();

            measurmentSample.Date = DateTime.Parse($"{measureSampleValues[0]} {measureSampleValues[1]}");

            measureSampleValues[2] = measureSampleValues[2].Replace(",", ".");
            measurmentSample.Pyranometr = double.Parse(measureSampleValues[2]);

            measureSampleValues[7] = measureSampleValues[7].Replace(",", ".");
            measurmentSample.AlternatingCurrent = double.Parse(measureSampleValues[7]);

            return measurmentSample;
        }
    }
}
