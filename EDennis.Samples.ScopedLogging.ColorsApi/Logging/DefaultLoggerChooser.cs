using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EDennis.AspNetCore.Base.Logging
{
    /// <summary>
    /// Default implementation of LoggerChooserSpec, which produces a
    /// KeyValuePair for User;
    /// </summary>
    public class DefaultLoggerChooser : LoggerChooser
    {
        public DefaultLoggerChooser(IEnumerable<ILogger<object>> loggers) : base(loggers) {
        }

        protected override IEnumerable<string> GetInputData(ScopeProperties scopeProperties) {
            return new string[] { $"User:{scopeProperties.User}" };
        }

        public override void AddCriterion(string user, int loggerIndex) => base.AddCriterion($"User:{user}", loggerIndex);

        public override void RemoveCriterion(string user, int loggerIndex) => base.RemoveCriterion($"User:{user}", loggerIndex);


    }
}
