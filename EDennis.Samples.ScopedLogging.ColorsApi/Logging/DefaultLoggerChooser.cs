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
        protected override IEnumerable<KeyValuePair<string, string>> 
                GetInputData(ScopeProperties scopeProperties) {
                    return new List<KeyValuePair<string, string>> {
                        KeyValuePair.Create("User", scopeProperties.User)
            };
        }

        public void AddCriterion(string user) => base.AddCriterion("User",user,1);

        public void RemoveCriterion(string user) => base.RemoveCriterion("User", user);


    }
}
