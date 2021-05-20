using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Doctr.Diagnostics;

namespace Doctr.Playground.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            Trace.WriteLine("WeatherForecast - GET");
            Debug.WriteLine("WeatherForecast (Debug) - GET");

            var rng = new Random();

            var forecast =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            Trace.WriteLine(forecast.FirstOrDefault().GetType().ReflectDump());
            Trace.WriteLine(forecast.Dump());

            var item = forecast.FirstOrDefault();
            Trace.WriteLine(item.Dump("_summary"));


            return forecast;
        }

        [HttpGet("{date}")]
        public WeatherForecast Get(DateTime date)
        {
            Trace.WriteLine(date);

            if(date < DateTime.UtcNow)
            {
                using(new FirstChanceExceptionHandler())
                {
                    var simulator = new ErrorSimulator();

                    simulator.SimulateError();
                }
            }

            return new WeatherForecast()
            {
                Date = date,
                     TemperatureC = 37,
                     Summary = "Warm"
            };
        }
    }

    public class ErrorSimulator
    {

        public void SimulateError()
        {
            throw new Exception("Simple Error");
        }

    }
}
