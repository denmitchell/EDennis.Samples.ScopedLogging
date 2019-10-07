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
    /// <summary>
    /// Sample subclass of LoggersControllerBase
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoggersController : LoggersControllerBase {

        public LoggersController(IEnumerable<ILogger<object>> loggers, ILoggerChooser loggerChooser)
            :base (loggers, loggerChooser){}

    }

}