using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.AspNetCore.Base.Logging
{
    /// <summary>
    /// SINGLETON -- HOLDS SPECS FOR WHICH LOGGER IS USED
    /// </summary>
    public abstract class LoggerChooser : ILoggerChooser
    {

        public static int DefaultIndex = 0;
        private readonly IEnumerable<ILogger<object>> _loggers;

        private readonly Dictionary<string, int> _settings
            = new Dictionary<string, int>();

        private readonly Dictionary<int, bool> _enabled
            = new Dictionary<int, bool>();

        //performance optimization to reduce later processing work
        private readonly Dictionary<string, int> _enabledSettings
            = new Dictionary<string, int>();


        private bool _isReset = true;

        public virtual bool IsEnabled(int loggerIndex) => _enabled[loggerIndex];

        public LoggerChooser(IEnumerable<ILogger<object>> loggers) {
            _loggers = loggers;
            Reset();
        }

        public virtual void Enable(int loggerIndex) {
            _enabled[loggerIndex] = true;
            ResetEnabledSettings();
            _isReset = false;
        }

        public virtual void Disable(int loggerIndex) {
            _enabled[loggerIndex] = false;
            ResetEnabledSettings();
            if (!_enabled.Where(e => e.Key != DefaultIndex).Any(e => e.Value))
                _isReset = true;
        }

        private void ResetEnabledSettings() {
            _enabledSettings.Clear();
            _settings
                .Where(s => IsEnabled(s.Value))
                .Select(s => s.Key)
                .ToList()
                .ForEach(k => _enabledSettings.Add(k, _settings[k]));
        }

        protected abstract IEnumerable<string> GetInputData(ScopeProperties scopeProperties);


        public virtual void AddCriterion(string scopePropertiesEntry, int loggerIndex) {
            if (!_settings.ContainsKey(scopePropertiesEntry)) {
                _settings.Add(scopePropertiesEntry, loggerIndex);
            } else
                _settings[scopePropertiesEntry] = loggerIndex;
        }

        public virtual void RemoveCriterion(string scopePropertiesEntry, int loggerIndex) {
            if (_settings.ContainsKey(scopePropertiesEntry))
                _settings.Remove(scopePropertiesEntry);
        }


        public virtual void ClearCriteria(int loggerIndex) {
            foreach (var entry in _settings.Where(s => s.Value == loggerIndex)) {
                _settings.Remove(entry.Key);
            }
        }

        public virtual void Reset() {
            _settings.Clear();
            _loggers.ToList().ForEach(a => _enabled.Add(_enabled.Count(), false));
            _settings.Add("*", DefaultIndex);
            Enable(DefaultIndex);
            _isReset = true;
        }


        public virtual int GetLoggerIndex(ScopeProperties scopeProperties) {

            //for speed, short-circuit all processing if default index will be returned.
            if (_isReset)
                return DefaultIndex;

            var scopePropertiesEntries = GetInputData(scopeProperties);
            foreach (var s in _enabledSettings)
                foreach (var sp in scopePropertiesEntries)
                    if (s.Key == sp)
                        return s.Value;

            return DefaultIndex;
        }

        public Dictionary<string, int> GetSettings() => _settings;


    }


}
