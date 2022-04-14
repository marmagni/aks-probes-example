using ProbesApi.Services;
using ProbesApi.HealthCheck;
using Microsoft.AspNetCore.Mvc;

namespace ProbesApi.Controllers
{
    [ApiController]
    [Route("simulation")]
    public class SimulationController : ControllerBase
    {
        readonly HeavyService heavyService;
        readonly ApiHealthState apiHealthState;

        public SimulationController(
            HeavyService heavyService,
            ApiHealthState apiHealthState)
        {
            this.heavyService = heavyService;
            this.apiHealthState = apiHealthState;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ApiHealthState request)
        {
            apiHealthState.IsHealthy = request.IsHealthy;
            apiHealthState.AutoRecoveryAfterSeconds = request.AutoRecoveryAfterSeconds;

            return Ok();
        }
    }
}
