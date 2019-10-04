using System.Collections.Generic;
using EDennis.AspNetCore.Base;

namespace EDennis.Samples.ScopedLogging.ColorsApi
{
    public interface ILoggerChooserSpec
    {
        IEnumerable<KeyValuePair<string, string>> GetRelevantKeyValuePairsFromScopeProperties(ScopeProperties scopeProperties);
        void SetLoggerIndex(ScopeProperties scopeProperties);
    }
}