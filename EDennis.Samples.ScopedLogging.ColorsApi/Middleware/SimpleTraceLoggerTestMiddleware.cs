using EDennis.AspNetCore.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.Samples.ScopedLogging.ColorsApi.Middleware
{


    public partial class SimpleTraceLoggerTestMiddleware
    {

        private readonly RequestDelegate _next;

        public SimpleTraceLoggerTestMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IServiceProvider provider, IWebHostEnvironment env) {

            if (!context.Request.Path.StartsWithSegments(new PathString("/swagger"))) {

                var scopeProperties = provider.GetRequiredService<ScopeProperties>();
                if (context.Request.Query.ContainsKey("logger")) {
                    scopeProperties.LoggerIndex = int.Parse(context.Request.Query["logger"]);
                }

            }

            await _next(context);

        }

    }

}
