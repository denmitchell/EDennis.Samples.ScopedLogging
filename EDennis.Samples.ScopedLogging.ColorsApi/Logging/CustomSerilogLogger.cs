using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using M = Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Logging
{
    public abstract class CustomSerilogLogger<T> : M.ILogger<T>
    {
        private readonly M.ILogger _logger;

        //public abstract LoggerConfiguration GetLoggerConfiguration();

        public CustomSerilogLogger(M.ILoggerFactory factory, IConfiguration configuration) {
            var simpleClassName = GetClassNameWithoutType();
            //var sink = configuration[$"Logging:Loggers:{simpleClassName}:Sink"];
            //var level = configuration[$"Logging:Loggers:{simpleClassName}:MinimumLevel"];
            var sloggerConfig = new Serilog.LoggerConfiguration();

            //if (level == "Trace" || level == "Verbose")
            //    sloggerConfig = sloggerConfig.MinimumLevel.Verbose();
            //else if (level == "Debug")
            //    sloggerConfig = sloggerConfig.MinimumLevel.Debug();
            //else if (level == "Information")
            //    sloggerConfig = sloggerConfig.MinimumLevel.Information();
            //else if (level == "Warning")
            //    sloggerConfig = sloggerConfig.MinimumLevel.Warning();
            //else if (level == "Error")
            //    sloggerConfig = sloggerConfig.MinimumLevel.Error();
            //else if (level == "Fatal" || level == "Critical")
            //    sloggerConfig = sloggerConfig.MinimumLevel.Fatal();

            var slogger = sloggerConfig            
                    .ReadFrom.Configuration(configuration, $"Logging:Loggers:{simpleClassName}")
                    //.WriteTo.Console()
                    .CreateLogger(); 

            var serilogLoggerFactory = new SerilogLoggerFactory(slogger);
            var category = typeof(T).Name;
            _logger = serilogLoggerFactory.CreateLogger(category);
        }

        IDisposable M.ILogger.BeginScope<TState>(TState state)
            => _logger.BeginScope(state);

        bool M.ILogger.IsEnabled(M.LogLevel logLevel)
            => _logger.IsEnabled(logLevel);

        void M.ILogger.Log<TState>(M.LogLevel logLevel,
                                 M.EventId eventId,
                                 TState state,
                                 Exception exception,
                                 Func<TState, Exception, string> formatter)
            => _logger.Log(logLevel, eventId, state, exception, formatter);

        private string GetClassNameWithoutType() {
            var name = this.GetType().Name;
            if (!this.GetType().IsGenericType) 
                return name;
            return name.Substring(0, name.IndexOf('`'));
        }

    }
}
