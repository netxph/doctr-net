using System;

namespace Doctr.Playground
{
    public class WeatherForecast
    {
        private string _summary;

        public WeatherForecast()
        {
            _summary = "Hello world!!!";
        }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary 
        { 
            get
            {
                return _summary;
            }
            set 
            {
                _summary = value;
            }
        }

    }
}
