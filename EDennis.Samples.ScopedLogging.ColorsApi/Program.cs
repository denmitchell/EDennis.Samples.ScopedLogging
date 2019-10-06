using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace EDennis.Samples.ScopedLogging.ColorsApi
{
    public class Program
    {
        public static Serilog.ILogger TraceLogger;
        public static Serilog.ILogger DebugLogger;
        public static void Main(string[] args) {

            //This is the default logger.
            //   Name = "Logger", Index = 0, LogLevel = Information
            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information() //note: Logging:LogLevel:Default overrides this setting
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .CreateLogger();

            Log.Logger.Information("Hello from Logger!");


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
