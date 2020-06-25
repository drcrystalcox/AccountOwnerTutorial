using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Contracts;

namespace AccountOwnerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

      //  private readonly ILogger<WeatherForecastController> _logger;
        private readonly ILoggerManager _logger;
        private IRepositoryWrapper _repoWrapper;


       // public WeatherForecastController(ILogger<WeatherForecastController> logger)
        public WeatherForecastController(ILoggerManager logger,IRepositoryWrapper repoWrapper)
        {
            _logger = logger;
            _repoWrapper=repoWrapper;
        }

        [HttpGet]
        /*public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        */
        public IEnumerable<string> Get()
        {
        _logger.LogInfo("Here is info message from the controller.");
        _logger.LogDebug("Here is debug message from the controller.");
        _logger.LogWarn("Here is warn message from the controller.");
        _logger.LogError("Here is error message from the controller.");


        var domesticAccounts = _repoWrapper.Account.FindByCondition(x => x.AccountType.Equals("Domestic"));
        var owners = _repoWrapper.Owner.FindAll();
 
        return new string[] { "value1", "value2" };
    }
    }
}
