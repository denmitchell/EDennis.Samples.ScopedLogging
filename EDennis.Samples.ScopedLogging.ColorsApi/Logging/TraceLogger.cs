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
    public class TraceLogger<T> : M.ILogger<T>
    {
        private readonly M.ILogger _logger;
        private static readonly SerilogLoggerFactory SerilogLoggerFactory;

        static TraceLogger() {
            var slogger = new S.LoggerConfiguration()
                        .MinimumLevel.Verbose()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()
                        .CreateLogger();
            SerilogLoggerFactory = new SerilogLoggerFactory(slogger);
        }

        public TraceLogger(M.ILoggerFactory factory) {
            var category = typeof(T).Name;
            _logger = SerilogLoggerFactory.CreateLogger(category);
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
    }
}
