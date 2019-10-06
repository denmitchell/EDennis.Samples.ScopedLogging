using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Base;

namespace EDennis.AspNetCore.Base.Logging
{
    /// <summary>
    /// Default implementation of LoggerChooserSpec, which produces a
    /// KeyValuePair for User;
    /// </summary>
    public class DefaultLoggerChooser : LoggerChooser
    {
        public const int SECONDARY_LOGGER_INDEX = 1;
        protected override IEnumerable<string> GetInputData(ScopeProperties scopeProperties) {
            return new string[] { $"User:{scopeProperties.User}" };
        }

        public void AddCriterion(string user) => base.AddCriterion($"User:{user}", SECONDARY_LOGGER_INDEX);

        public void RemoveCriterion(string user) => base.RemoveCriterion($"User:{user}", SECONDARY_LOGGER_INDEX);


    }
}
