using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using authorization_play.Core.Models;
using authorization_play.Core.Resources.Models;
using authorization_play.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace authorization_play.TestApi.Controllers
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
        //[Authorize]
        [AuthorizePermissionTicket]
        public IActionResult Get()
        {
            // somehow determine what the resource, action and schema are
            var resource = CRN.FromValue("crn:farm/*");
            var action = ResourceAction.FromValue("iam:owner");
            var schema = DataSchema.FromValue("ag-data:farm");

            // validate permissions
            var permissionsValid = this.ValidatePermissions(resource, action, schema);
            if (!permissionsValid) return Forbid();

            var rng = new Random();
            return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray());
        }
    }
}
