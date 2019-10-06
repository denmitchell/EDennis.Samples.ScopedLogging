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
        protected override IEnumerable<string> GetInputData(ScopeProperties scopeProperties) {
            return new string[] { $"User:{scopeProperties.User}" };
        }

        public override void AddCriterion(string user, int loggerIndex) => base.AddCriterion($"User:{user}", loggerIndex);

        public override void RemoveCriterion(string user, int loggerIndex) => base.RemoveCriterion($"User:{user}", loggerIndex);


    }
}
