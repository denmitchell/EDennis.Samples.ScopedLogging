using EDennis.AspNetCore.Base.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base
{
    public class ScopeProperties
    {
        private readonly ILoggerChooser _loggerChooser;
            
        public ScopeProperties(ILoggerChooser loggerChooser = null) {
            _loggerChooser = loggerChooser;
            if (loggerChooser != null) {
                UpdateLoggerIndex();
                loggerChooser.Changed += OnLoggerChooserChange;
            }
        }
        public int LoggerIndex { get; set; } = 0;
        public string User { get; set; }
        public Claim[] Claims { get; set; }
        public Dictionary<string, object> OtherProperties { get; set; }
            = new Dictionary<string, object>();


        public void UpdateLoggerIndex() {
            if (_loggerChooser != null) {
                if (_loggerChooser.Enabled)
                    _loggerChooser.SetLoggerIndex(this);
                else
                    LoggerIndex = 0; //default
            }
        }


        private void OnLoggerChooserChange(object sender, EventArgs e) {
            UpdateLoggerIndex();
        }



    }
}
