using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Logging
{
    public static class ILoggerExtensions
    {
        public static LogLevel EnabledAt<T>(this ILogger<T> logger) {
            for (int i = (int)LogLevel.Trace; i < (int)LogLevel.None; i++)
                if (logger.IsEnabled((LogLevel)i))
                    return (LogLevel)i;

            return LogLevel.None;
        }
        public static LogLevel EnabledAt(this ILogger logger) {
            for (int i = (int)LogLevel.Trace; i < (int)LogLevel.None; i++)
                if (logger.IsEnabled((LogLevel)i))
                    return (LogLevel)i;

            return LogLevel.None;
        }
    }
}
