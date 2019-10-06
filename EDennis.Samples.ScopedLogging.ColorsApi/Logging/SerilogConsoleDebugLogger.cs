using Microsoft.Extensions.Configuration;
using M = Microsoft.Extensions.Logging;
using S = Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace EDennis.Samples.ScopedLogging.ColorsApi.Logging
{
    public class SerilogConsoleDebugLogger<T> : CustomSerilogLogger<T>
    {
        public override LoggerConfiguration GetLoggerConfiguration() {
            return new S.LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console();
        }
        public TraceLogger(M.ILoggerFactory factory) : base(factory){}

    }
}
