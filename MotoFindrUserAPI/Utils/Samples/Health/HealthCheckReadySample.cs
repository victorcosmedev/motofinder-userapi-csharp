using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Health
{
    public class HealthCheckReadySample : IExamplesProvider<HealthCheckResponseDto>
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
                        Name = "oracle_query",
                        Status = "Healthy",
                        Description = "Banco de dados esta online",
                        Error = null
                    }
                }
            };
        }
    }
}
