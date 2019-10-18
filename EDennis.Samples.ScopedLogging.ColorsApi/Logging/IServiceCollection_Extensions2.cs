using Castle.DynamicProxy;
using EDennis.AspNetCore.Base;
using EDennis.Samples.ScopedLogging.ColorsApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDennis.Samples.ScopedLogging.ColorsApi {
    public static class IServiceCollection_Extensions2 {

        public static IServiceCollection AddScopedTraceable<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface => services.AddTraceable<TInterface, TImplementation>(ServiceLifetime.Scoped);

        public static IServiceCollection AddSingletonTraceable<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface => services.AddTraceable<TInterface, TImplementation>(ServiceLifetime.Singleton);

        public static IServiceCollection AddTransientTraceable<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : TInterface => services.AddTraceable<TInterface, TImplementation>(ServiceLifetime.Transient);


        public static IServiceCollection AddScopedTraceable<TImplementation>(this IServiceCollection services)
            where TImplementation : class => services.AddTraceable<TImplementation, TImplementation>(ServiceLifetime.Scoped);

        public static IServiceCollection AddSingletonTraceable<TImplementation>(this IServiceCollection services)
            where TImplementation : class => services.AddTraceable<TImplementation, TImplementation>(ServiceLifetime.Singleton);

        public static IServiceCollection AddTransientTraceable<TImplementation>(this IServiceCollection services)
            where TImplementation : class => services.AddTraceable<TImplementation, TImplementation>(ServiceLifetime.Transient);


        public static IServiceCollection AddTraceable<TInterface,TImplementation>(
            this IServiceCollection services, ServiceLifetime serviceLifetime)
            where TInterface : class
            where TImplementation: TInterface {

            var constructorParameters = typeof(TImplementation)
                .GetConstructors()
                .FirstOrDefault()
                .GetParameters()
                .Select(x => x.ParameterType)
                .ToArray();

            Type loggerType = constructorParameters
                .FirstOrDefault(t => typeof(ILogger).IsAssignableFrom(t));
            Type loggersType = GetIEnumerableType(loggerType);

            object[] args = new object[constructorParameters.Count()];

            services.Add(new ServiceDescriptor(typeof(TInterface),f => {
                var loggers = f.GetRequiredService(loggersType) as IEnumerable<object>;
                var scopeProperties = f.GetRequiredService<ScopeProperties>();
                ILogger activeLogger = (ILogger)loggers.ElementAt(scopeProperties.LoggerIndex);
                for (int i = 0; i < args.Length; i++) {
                    args[i] = f.GetRequiredService(constructorParameters[i]);
                }
                var proxy = (TImplementation)new ProxyGenerator()
                    .CreateClassProxy(typeof(TImplementation), args,
                        new TraceInterceptor(activeLogger, scopeProperties));

            //return services.AddScoped(p => {
                return proxy;
            }, serviceLifetime));

            return services;
        }





        public static Type GetIEnumerableType<T>(T type)
            where T: Type {

            var iEnumerableType = typeof(IEnumerable<>);
            var constructedIEnumerableType = iEnumerableType.MakeGenericType(type);

            return constructedIEnumerableType;
            //var instance = Activator.CreateInstance(constructedIEnumerableType);
        }

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
