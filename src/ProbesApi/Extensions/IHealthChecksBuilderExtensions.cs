using System;
using ProbesApi.HealthCheck;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ProbesApi.Extensions
{
    public static class IHealthChecksBuilderExtensions
    {
        public static IHealthChecksBuilder AddCustomHealthCheck(this IHealthChecksBuilder builder, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default, TimeSpan? timeout = default)
        {
            IHealthCheck hc(IServiceProvider sp) => new CustomHealthCheck(sp.GetService<ApiHealthState>());

            return builder.Add(new HealthCheckRegistration(name ?? "customcheck", hc, failureStatus, tags, timeout));
        }
    }
}
