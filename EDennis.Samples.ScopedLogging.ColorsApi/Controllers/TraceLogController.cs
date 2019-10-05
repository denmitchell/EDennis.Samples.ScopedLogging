using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EDennis.Samples.ScopedLogging.ColorsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraceLogController : ControllerBase {
        private readonly ILoggerChooser _loggerChooser;

        public TraceLogController(ILoggerChooser loggerChooser) {
            _loggerChooser = loggerChooser;
        }

        [HttpPost("[action]/{user}")]
        public void Enable(string user) {
            _loggerChooser.AddCriterion("User", user, 1);
            _loggerChooser.Enabled = true;
        }

        [HttpPost("[action]")]
        public void Disable() {
            _loggerChooser.ClearCriteria();
            _loggerChooser.Enabled = false;
        }

    }
}