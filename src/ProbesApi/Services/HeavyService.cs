using System.Threading;
using Microsoft.Extensions.Logging;

namespace ProbesApi.Services
{
    public class HeavyService
    {
        public HeavyService(ILogger<HeavyService> logger)
        {
            logger.LogInformation("HeavyService constructor called!");
            Thread.Sleep(1000);            
        }
    }
}
