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
    public class SecondaryLogController : ControllerBase {
        private readonly IEnumerable<ILogger<SecondaryLogController>> _loggers;
        private readonly ILoggerChooser _loggerChooser;

        public SecondaryLogController(IEnumerable<ILogger<SecondaryLogController>> loggers, ILoggerChooser loggerChooser) {
            _loggers = loggers;
            _loggerChooser = loggerChooser;
        }

        [HttpPost("[action]/{user}/{loggerIndex}")]
        public IActionResult Enable(string user, int loggerIndex) {
            _loggerChooser.AddCriterion($"User:{user}", loggerIndex);
            _loggerChooser.Enabled = loggerIndex;
            return GetCurrentCriteria();
        }


        [HttpPost("[action]/{user}/{loggerIndex}")]
        public IActionResult Add(string user, int loggerIndex) {
            _loggerChooser.AddCriterion($"User:{user}", loggerIndex);
            return GetCurrentCriteria();
        }


        [HttpPost("[action]/{loggerIndex}")]
        public IActionResult Enable(int loggerIndex) {
            _loggerChooser.Enabled = loggerIndex;
            return GetCurrentCriteria();
        }

        [HttpPost("[action]/{user}/{loggerIndex}")]
        public IActionResult Remove(string user, int loggerIndex) {
            _loggerChooser.RemoveCriterion($"User:{user}", loggerIndex);
            return GetCurrentCriteria();
        }


        [HttpPost("[action]/{loggerIndex}")]
        public IActionResult Clear(int loggerIndex) {
            _loggerChooser.ClearCriteria(loggerIndex);
            return GetCurrentCriteria();
        }




        [HttpPost("[action]")]
        public IActionResult Reset() {
            _loggerChooser.ClearCriteria();
            _loggerChooser.Enabled = ILoggerChooser.DefaultIndex;
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