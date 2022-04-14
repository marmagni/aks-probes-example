using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ProbesApi.HealthCheck
{
    public static class CustomResponseWriter
    {
        public static async Task Writer(HttpContext context, HealthReport healthReport)
        {
            context.Response.ContentType = "application/json";

            var result = JsonConvert.SerializeObject(new
            {
                statusApplication = healthReport.Status.ToString(),
                healthChecks = healthReport.Entries.Select(e => new
                {
                    check = e.Key,
                    status = e.Value.Status.ToString(),
                    errorMessage = e.Value.Exception?.Message
                })
            });

            await context.Response.WriteAsync(result);
        }
    }
}
