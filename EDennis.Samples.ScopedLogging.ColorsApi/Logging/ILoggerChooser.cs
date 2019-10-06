using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Logging
{
    public interface ILoggerChooser
    {
        static int DefaultIndex;
        bool IsEnabled(int loggerIndex);
        void Enable(int loggerIndex);
        void Disable(int loggerIndex);
        void AddCriterion(string scopePropertiesEntry, int loggerIndex);
        void RemoveCriterion(string scopePropertiesEntry, int loggerIndex);
        void ClearCriteria(int loggerIndex);
        void Reset();
        int GetLoggerIndex(ScopeProperties scopeProperties);
        Dictionary<string, int> GetSettings();
    }
}