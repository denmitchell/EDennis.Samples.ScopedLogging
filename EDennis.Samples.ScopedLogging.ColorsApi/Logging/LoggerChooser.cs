using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Base.Logging
{
    /// <summary>
    /// SINGLETON -- HOLDS SPECS FOR WHICH LOGGER IS USED
    /// </summary>
    public abstract class LoggerChooser : ILoggerChooser
    {
        private readonly Dictionary<string, int> _settings 
            = new Dictionary<string, int>() { { "*", DefaultIndex } };

        public bool Enabled { get; set; }

        public static int DefaultIndex = 0;

        protected abstract IEnumerable<string> GetInputData(ScopeProperties scopeProperties);


        public virtual void AddCriterion(string scopePropertiesEntry, int loggerIndex) {
            if (_settings.Count() == 0 || !_settings.ContainsKey(scopePropertiesEntry))
                _settings.Add(scopePropertiesEntry, loggerIndex);
            else
                _settings[scopePropertiesEntry] = loggerIndex;
        }

        public virtual void RemoveCriterion(string scopePropertiesEntry, int loggerIndex) {
            if (_settings.ContainsKey(scopePropertiesEntry))
                _settings.Remove(scopePropertiesEntry);
        }

        public virtual void ClearCriteria() {
            _settings.Clear();
            _settings.Add("*", DefaultIndex);
        }


        public virtual int GetLoggerIndex(ScopeProperties scopeProperties) {
            var scopePropertiesEntries = GetInputData(scopeProperties);
            foreach (var s in _settings)
                foreach (var sp in scopePropertiesEntries)
                    if (s.Key == sp)
                        return s.Value;
            return DefaultIndex;
        }

        public Dictionary<string, int> GetSettings() => _settings;


    }


}
