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
            if (loggerChooser != null)
                UpdateLoggerIndex();            
        }

        public int LoggerIndex { get; set; } = 0;
        public string User { get; set; }
        public Claim[] Claims { get; set; }
        public Dictionary<string, object> OtherProperties { get; set; }
            = new Dictionary<string, object>();


        public void UpdateLoggerIndex() {
                LoggerIndex = _loggerChooser?.Enabled ?? ILoggerChooser.DefaultIndex; 
        }


    }
}
