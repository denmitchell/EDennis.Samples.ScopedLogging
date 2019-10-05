using System;
using System.Collections.Generic;
using EDennis.AspNetCore.Base;

namespace EDennis.AspNetCore.Base.Logging
{
    public interface ILoggerChooser
    {
        void AddCriterion(string scopePropertiesKey, string scopePropertiesValue, int loggerIndex = 1);
        void RemoveCriterion(string scopePropertiesKey, string scopePropertiesValue);
        void ClearCriteria();

        void SetLoggerIndex(ScopeProperties scopeProperties);
        bool Enabled { get; set; }

        event EventHandler Changed;
    }
}