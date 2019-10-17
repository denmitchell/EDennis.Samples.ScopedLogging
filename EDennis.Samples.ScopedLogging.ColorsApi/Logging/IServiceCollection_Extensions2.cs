using Castle.DynamicProxy;
using EDennis.AspNetCore.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.Samples.ScopedLogging.ColorsApi {
    public static class IServiceCollection_Extensions2 {

        public static IServiceCollection AddScoped<TRepo,TContext>(this IServiceCollection services)
            where TRepo : class
            where TContext : DbContext {
            return services.AddScoped(f => {
                var loggers = f.GetRequiredService<IEnumerable<ILogger<TRepo>>>();
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);
                var context = f.GetRequiredService<TContext>();
                var repo = (TRepo)new ProxyGenerator()
                    .CreateClassProxy(typeof(TRepo),
                        new object[] { context, scopeProperties, activeLogger },
                        new TraceInterceptor(activeLogger, scopeProperties));
                return repo;
            });

        }
    }
}
