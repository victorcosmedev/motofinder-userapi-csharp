using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Utils.Doc;
using MotoFindrUserAPI.Utils.Samples.Health;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Presentation.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet("live")]
        [SwaggerOperation(
            Summary = ApiDoc.LiveCheckSummary,
            Description = ApiDoc.LiveCheckDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "API está online", typeof(HealthCheckResponseDto))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "API não está saudável", typeof(HealthCheckResponseDto))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(HealthCheckLiveSample))]
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
        [SwaggerOperation(
            Summary = ApiDoc.ReadyCheckSummary,
            Description = ApiDoc.ReadyCheckDescription
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "API e dependências estão prontas", typeof(HealthCheckResponseDto))]
        [SwaggerResponse(StatusCodes.Status503ServiceUnavailable, "API ou dependências não estão prontas", typeof(HealthCheckResponseDto))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(HealthCheckReadySample))]
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
