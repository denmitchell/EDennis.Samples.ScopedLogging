using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace EDennis.AspNetCore.Base.Logging
{
    public interface ILoggerChooser
    {
        static int DefaultIndex;
        bool Enabled { get; set; }
        void AddCriterion(string scopePropertiesEntry, int loggerIndex);
        void ClearCriteria();
        void RemoveCriterion(string scopePropertiesEntry, int loggerIndex);
        int GetLoggerIndex(ScopeProperties scopeProperties);
        Dictionary<string, int> GetSettings();
    }
}