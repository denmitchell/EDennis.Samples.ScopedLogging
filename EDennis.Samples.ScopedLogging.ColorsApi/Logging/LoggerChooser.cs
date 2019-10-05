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
    public abstract class LoggerChooser : List<LoggerChooserEntry>, ILoggerChooser
    {

        protected abstract IEnumerable<KeyValuePair<string, string>>
            GetInputData(ScopeProperties scopeProperties);

        protected bool _enabled;

        public event EventHandler Changed;

        public bool Enabled {
            get {
                return _enabled;
            }
            set {
                _enabled = value;

                Changed?.Invoke(this, null);
            }
        }

        public void SetLoggerIndex(ScopeProperties scopeProperties) {

            var keyValuePairs = GetInputData(scopeProperties);

            var loggerIndex = this
                .Where(x => keyValuePairs
                    .Any(m => m.Key.Equals(x.ScopePropertiesKey, StringComparison.OrdinalIgnoreCase)
                            && m.Value.Equals(x.ScopePropertiesValue, StringComparison.OrdinalIgnoreCase)))
                .FirstOrDefault()
                ?.LoggerIndex ?? 0;

            scopeProperties.LoggerIndex = loggerIndex;

        }


        public virtual void AddCriterion(string scopePropertiesKey, string scopePropertiesValue, int loggerIndex = 1) {
            for (int i = 0; i < this.Count; i++) {
                if (this[i].ScopePropertiesKey == scopePropertiesKey
                       && this[i].ScopePropertiesValue == scopePropertiesValue) {
                    this[i].LoggerIndex = loggerIndex;
                    break;
                } else {
                    Add(new LoggerChooserEntry { 
                        ScopePropertiesKey = scopePropertiesKey,
                        ScopePropertiesValue = scopePropertiesValue,
                        LoggerIndex = loggerIndex
                    });
                }
            }
        }

        public virtual void RemoveCriterion(string scopePropertiesKey, string scopePropertiesValue) {
            for (int i = 0; i < this.Count; i++) {
                if (this[i].ScopePropertiesKey == scopePropertiesKey
                       && this[i].ScopePropertiesValue == scopePropertiesValue) {
                    RemoveAt(i);
                    break;
                }
            }
        }
        public virtual void ClearCriteria() => Clear();

    }

    public class LoggerChooserEntry
    {
        public string ScopePropertiesKey { get; set; }
        public string ScopePropertiesValue { get; set; }
        public int LoggerIndex { get; set; }
    }

}
