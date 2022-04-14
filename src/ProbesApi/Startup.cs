using System;
using Serilog;
using System.Threading;
using ProbesApi.Services;
using ProbesApi.Extensions;
using ProbesApi.HealthCheck;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProbesApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<HealthCheckDocumentFilter>();

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Probes Api", Version = "v1" });
            });

            services.AddSingleton(services);
            services.AddSingleton<HeavyService>();
            services.AddSingleton<WarmupService>();
            services.AddSingleton<ApiHealthState>();
            services.AddSingleton<StartupService>();

            services.AddHealthChecks().AddCustomHealthCheck();
        }

        public void Configure(IApplicationBuilder app, IHostApplicationLifetime applicationLifetime)
        {
            var deadlockAfterSec = Configuration.GetValue<int>("DEADLOCK_AFTER_SECONDS");
            var startupLatencySec = Configuration.GetValue<int>("STARTUP_LATENCY_SECONDS");
            
            Thread.Sleep(startupLatencySec * 1000);

            app.UseDeveloperExceptionPage();
            app.UseSwagger(setupAction: null);

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Probes Api v1");
            });

            app.Use(async (context, next) =>
            {
                if (deadlockAfterSec > 0)
                {
                    var startDate = app.ApplicationServices.GetService<StartupService>();
                    var crashTime = startDate.StartedDate.AddSeconds(deadlockAfterSec);

                    if (crashTime < DateTime.Now)
                    {
                        Thread.Sleep(int.MaxValue);
                    }
                }

                await next.Invoke();
            });

            app.UseRouting();

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/alive", new() { Predicate = _ => false });
                endpoints.MapHealthChecks("/health", new() { ResponseWriter = CustomResponseWriter.Writer });
            });

            app.Map("/warmup", app =>
            {
                app.Run(async context =>
                {
                    var provider = context.RequestServices;
                    var warmupService = provider.GetService<WarmupService>();

                    warmupService.CreateInstances();

                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("Done");
                });
            });
        }
    }
}
