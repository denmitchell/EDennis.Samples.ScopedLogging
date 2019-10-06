using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.Samples.ScopedLogging.ColorsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraceLogController : ControllerBase {
        private readonly IEnumerable<ILogger<TraceLogController>> _loggers;
        private readonly ILoggerChooser _loggerChooser;

        public TraceLogController(IEnumerable<ILogger<TraceLogController>> loggers, ILoggerChooser loggerChooser) {
            _loggers = loggers;
            _loggerChooser = loggerChooser;
        }

        [HttpPost("[action]/{user}")]
        public IActionResult Enable(string user) {
            _loggerChooser.AddCriterion($"User:{user}", 1);
            _loggerChooser.Enabled = true;
            return GetCurrentCriteria();
        }

        [HttpPost("[action]")]
        public IActionResult Disable() {
            _loggerChooser.ClearCriteria();
            _loggerChooser.Enabled = false;
            return GetCurrentCriteria();
        }

        [HttpGet]
        public IActionResult GetCurrentCriteria() {

            List<dynamic> settings = new List<dynamic>();
            for (int i = 0; i < _loggers.Count(); i++) {
                settings.Add(new {
                    Logger = _loggers.ElementAt(i).GetType().Name,
                    LogLevel = _loggers.ElementAt(i).EnabledAt().ToString(),
                    EnabledFor = _loggerChooser.GetSettings()
                        .Where(s => s.Value == i)
                        .Select(s => s.Key)
                        .ToList()
                }); 
            }
            return Ok(settings);
        }

    }

}