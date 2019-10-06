using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using M = Microsoft.Extensions.Logging;
using S = Serilog;

namespace EDennis.AspNetCore.Base.Logging
{    
    
    /// <summary>
    /// A Serilog logger that prints debug (and higher-level) logs to the console
    /// </summary>
    /// <typeparam name="T">Class into which the logger is injected</typeparam>
    public class SerilogConsoleDebugLogger<T> : CustomSerilogLogger<T>
    {
        /// <summary>
        /// The configuration for this logger ... could come from appsettings, if desired.
        /// </summary>
        /// <returns></returns>
        //public override LoggerConfiguration GetLoggerConfiguration() {
        //    return new S.LoggerConfiguration()
        //                .MinimumLevel.Debug()
        //                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        //                .Enrich.FromLogContext()
        //                .WriteTo.Console();
        //}
        public SerilogConsoleDebugLogger(M.ILoggerFactory factory, IConfiguration configuration) : base(factory, configuration){}

    }
}
