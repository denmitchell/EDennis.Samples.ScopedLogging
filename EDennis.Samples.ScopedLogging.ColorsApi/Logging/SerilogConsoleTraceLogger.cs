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
    public class SerilogConsoleTraceLogger<T> : CustomSerilogLogger<T>
    {
        public override LoggerConfiguration GetLoggerConfiguration() {
            return new S.LoggerConfiguration()
                        .MinimumLevel.Verbose()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console();
        }
        public SerilogConsoleTraceLogger(M.ILoggerFactory factory) : base(factory){}

    }
}
