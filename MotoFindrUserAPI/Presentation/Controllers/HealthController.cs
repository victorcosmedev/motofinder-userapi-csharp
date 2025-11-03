using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;

namespace MotoFindrUserAPI.Presentation.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet("live")]
        public async Task<IActionResult> Live(CancellationToken ct)
        {
            var report = await _healthCheckService.CheckHealthAsync(
                r => r.Tags.Contains("live"), ct);

            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    error = e.Value.Exception?.Message
                })
            };

            return report.Status == HealthStatus.Healthy
                ? Ok(result)
                : StatusCode(503, result);
        }



        [HttpGet("ready")]
        public async Task<IActionResult> Ready(CancellationToken ct)
        {
            var report = await _healthCheckService.CheckHealthAsync(r => r.Tags.Contains("ready"), ct);


            var result = new
            {
                status = report.Status.ToString(),
                checks = report.Entries.Select(e => new
                {
                    name = e.Key,
                    status = e.Value.Status.ToString(),
                    description = e.Value.Description,
                    error = e.Value.Exception?.Message
                })
            };

            return report.Status == HealthStatus.Healthy ? Ok(result) : StatusCode(503, result);
        }
    }
}
