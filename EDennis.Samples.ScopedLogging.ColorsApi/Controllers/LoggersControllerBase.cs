using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Logging
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class LoggersControllerBase : ControllerBase {
        private readonly IEnumerable<ILogger<object>> _loggers;
        private readonly ILoggerChooser _loggerChooser;

        public LoggersControllerBase(IEnumerable<ILogger<object>> loggers, ILoggerChooser loggerChooser) {
            _loggers = loggers;
            _loggerChooser = loggerChooser;
        }

        private int GetLoggerIndex(string loggerNameOrIndex) {

            var isInt = int.TryParse(loggerNameOrIndex, out int index);
            if (isInt)
                return index;

            for (int i = 0; i < _loggers.Count(); i++)
                if (GetLoggerName(i).Equals(loggerNameOrIndex, StringComparison.OrdinalIgnoreCase))
                    return i;

            return -1;
        }

        [HttpPost("[action]/{loggerNameOrIndex}/{user}")]
        public IActionResult Enable(string loggerNameOrIndex, string user) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.AddCriterion(user, loggerIndex);
            _loggerChooser.Enable(loggerIndex);
            return GetLoggers();
        }

        [HttpPost("[action]/{loggerNameOrIndex}")]
        public IActionResult Enable(string loggerNameOrIndex) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.Enable(loggerIndex);
            return GetLoggers();
        }

        [HttpPost("[action]/{loggerNameOrIndex}")]
        public IActionResult Disable(string loggerNameOrIndex) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.Disable(loggerIndex);
            return GetLoggers();
        }


        [HttpPost("[action]/{loggerNameOrIndex}/{user}")]
        public IActionResult Add(string loggerNameOrIndex, string user) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.AddCriterion(user, loggerIndex);
            return GetLoggers();
        }


        [HttpPost("[action]/{loggerNameOrIndex}/{user}")]
        public IActionResult Remove(string loggerNameOrIndex, string user) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.RemoveCriterion(user, loggerIndex);
            return GetLoggers();
        }


        [HttpPost("[action]/{loggerNameOrIndex}")]
        public IActionResult Clear(string loggerNameOrIndex) {
            var loggerIndex = GetLoggerIndex(loggerNameOrIndex);
            _loggerChooser.ClearCriteria(loggerIndex);
            return GetLoggers();
        }




        [HttpPost("[action]")]
        public IActionResult Reset() {
            _loggerChooser.Reset();
            return GetLoggers();
        }


        [HttpGet]
        public IActionResult GetLoggers() {

            List<dynamic> settings = new List<dynamic>();
            for (int i = 0; i < _loggers.Count(); i++) {
                settings.Add(new {
                    Name = GetLoggerName(i),
                    Index = i,
                    LogLevel = _loggers.ElementAt(i).EnabledAt().ToString(),
                    Enabled = _loggerChooser.IsEnabled(i),
                    Criteria = _loggerChooser.GetSettings()
                        .Where(s => s.Value == i)
                        .Select(s => s.Key)
                        .ToList()
                }); 
            }
            return Ok(settings);
        }

        private string GetLoggerName(int loggerIndex) {
            var name = _loggers.ElementAt(loggerIndex).GetType().Name;
            var endOfName = name.IndexOf("`");
            if (endOfName == -1)
                endOfName = name.Length;
            return name.Substring(0,endOfName);
        }

    }

}