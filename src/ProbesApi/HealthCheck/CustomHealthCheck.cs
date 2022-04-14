using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ProbesApi.HealthCheck
{
    public class CustomHealthCheck : IHealthCheck
    {
        readonly ApiHealthState apiHealthState;
        public CustomHealthCheck(ApiHealthState apiHealthState)
        {
            this.apiHealthState = apiHealthState;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (apiHealthState.IsHealthy)
                {
                    return HealthCheckResult.Healthy();
                }
                else
                {
                    throw new Exception("This dependency isn't working");
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(exception: ex);
            }
        }
    }
}
