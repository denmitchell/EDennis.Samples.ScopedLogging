using Castle.DynamicProxy;
using EDennis.AspNetCore.Base;
using EDennis.AspNetCore.Base.Logging;
using EDennis.AspNetCore.Base.Testing;
using EDennis.Samples.ScopedLogging.ColorsApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDennis.Samples.ScopedLogging.ColorsApi {
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public IServiceCollection Services { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();


            services.AddScoped<ScopeProperties>();

            services.AddDbContext<ColorDbContext>(options =>
                            options.UseSqlite($"Data Source={Environment.ContentRootPath}/color.db")
                            );

            //add secondary loggers for on-demand, per-user verbose and debug logging
            services.AddSecondaryLoggers(typeof(SerilogVerboseLogger<>), typeof(SerilogDebugLogger<>));

            //add ColorRepo as Scoped Castle.Core.DynamicProxy with TraceInterceptor
            services.AddScopedTraceable<ColorRepo>();

            #region old

            //services.AddSingleton(typeof(ILogger<>), typeof(SerilogVerboseLogger<>));
            //services.AddSingleton(typeof(ILogger<>), typeof(SerilogDebugLogger<>));


            //services.AddSingleton<ILoggerChooser>(f => {
            //    var loggers = f.GetRequiredService<IEnumerable<ILogger<object>>>();
            //    return new DefaultLoggerChooser(loggers);
            //});

            //services.AddScoped<ColorRepo, ColorDbContext>();
            //services.AddScoped<ColorRepo>(f => {
            //    var loggers = f.GetRequiredService<IEnumerable<ILogger<ColorRepo>>>();
            //    var scopeProperties = f.GetRequiredService<ScopeProperties>();
            //    var activeLogger = loggers.ElementAt(scopeProperties.LoggerIndex);
            //    var context = f.GetRequiredService<ColorDbContext>();
            //    var repo = (ColorRepo)new ProxyGenerator()
            //        .CreateClassProxy(typeof(ColorRepo),
            //            new object[] { context, scopeProperties, activeLogger },
            //            new TraceInterceptor(activeLogger,scopeProperties));
            //    return repo;
            //});

            #endregion

            if (Environment.EnvironmentName == "Development") {
                services.AddSwaggerGen(c => {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Color API", Version = "v1" });
                });
            }

            Services = services;

            //ServiceProvider = services.BuildServiceProvider();

            services.AddAuthentication(options => {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseAutoLogin();

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            if (env.EnvironmentName == "Development") {
                app.UseSwagger();

                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Color API V1");
                });
            }

        }
    }
}
