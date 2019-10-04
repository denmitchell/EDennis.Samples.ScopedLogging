using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;

namespace EDennis.Samples.ScopedLogging.ColorsApi
{
    /// <summary>
    /// Default implementation of LoggerChooserSpec, which produces a
    /// KeyValuePair for User;
    /// </summary>
    public class DefaultLoggerChooserSpec : LoggerChooserSpec
    {
        public override IEnumerable<KeyValuePair<string, string>> GetRelevantKeyValuePairsFromScopeProperties(ScopeProperties scopeProperties) {
            return new List<KeyValuePair<string, string>> {
                KeyValuePair.Create("User", scopeProperties.User)
            };
        }
    }
}
