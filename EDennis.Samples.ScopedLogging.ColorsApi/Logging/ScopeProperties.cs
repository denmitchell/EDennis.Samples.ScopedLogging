﻿using System.Collections.Generic;
using System.Security.Claims;

namespace EDennis.AspNetCore.Base
{
    public class ScopeProperties 
    {
        public int LoggerIndex { get; set; } = 0;
        public string User { get; set; }
        public Claim[] Claims { get; set; }
        public Dictionary<string, object> OtherProperties { get; set; }
            = new Dictionary<string, object>();


    }
}
