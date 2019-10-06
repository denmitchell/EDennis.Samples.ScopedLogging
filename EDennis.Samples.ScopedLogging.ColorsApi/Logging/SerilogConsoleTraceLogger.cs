using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using M = Microsoft.Extensions.Logging;
using S = Serilog;

namespace EDennis.AspNetCore.Base.Logging
{
    /// <summary>
    /// A Serilog logger that prints verbose (and higher-level) logs to the console
    /// </summary>
    /// <typeparam name="T">Class into which the logger is injected</typeparam>
    public class SerilogConsoleTraceLogger<T> : CustomSerilogLogger<T>
    {
    
        /// <summary>
        /// The configuration for this logger ... could come from appsettings, if desired.
        /// </summary>
        /// <returns></returns>
        //public override LoggerConfiguration GetLoggerConfiguration() {
        //    return new S.LoggerConfiguration()
        //                .MinimumLevel.Verbose()
        //                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        //                .Enrich.FromLogContext()
        //                .WriteTo.Console();
        //}


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="factory"></param>
        public SerilogConsoleTraceLogger(M.ILoggerFactory factory, IConfiguration configuration) : base(factory, configuration) {}

    }
}
