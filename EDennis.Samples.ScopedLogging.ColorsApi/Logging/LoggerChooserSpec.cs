using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.Samples.ScopedLogging.ColorsApi
{
    /// <summary>
    /// SINGLETON -- HOLDS SPECS FOR WHICH LOGGER IS USED
    /// </summary>
    public abstract class LoggerChooserSpec : List<LoggerChooserSpecEntry>, ILoggerChooserSpec
    {

        public abstract IEnumerable<KeyValuePair<string, string>>
            GetRelevantKeyValuePairsFromScopeProperties(ScopeProperties scopeProperties);

        public void SetLoggerIndex(ScopeProperties scopeProperties) {

            var keyValuePairs = GetRelevantKeyValuePairsFromScopeProperties(scopeProperties);

            var loggerIndex = this
                .Where(x => keyValuePairs
                    .Any(m => m.Key.Equals(x.ScopePropertiesKey, StringComparison.OrdinalIgnoreCase)
                            && m.Value.Equals(x.ScopePropertiesValue, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault()
                ?.LoggerIndex ?? 0;

            scopeProperties.LoggerIndex = loggerIndex;

        }
    }

    public class LoggerChooserSpecEntry
    {
        public string ScopePropertiesKey { get; set; }
        public string ScopePropertiesValue { get; set; }
        public int LoggerIndex { get; set; }
    }
}
