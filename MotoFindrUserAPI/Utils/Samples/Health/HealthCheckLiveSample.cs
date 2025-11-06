using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Health
{
    public class HealthCheckLiveSample : IExamplesProvider<HealthCheckResponseDto>
    {
        public HealthCheckResponseDto GetExamples()
        {
            return new HealthCheckResponseDto
            {
                Status = "Healthy",
                Checks = new List<HealthCheckItemDto>
                {
                    new HealthCheckItemDto
                    {
                        Name = "self",
                        Status = "Healthy",
                        Description = null,
                        Error = null
                    }
                }
            };
        }
    }
}
