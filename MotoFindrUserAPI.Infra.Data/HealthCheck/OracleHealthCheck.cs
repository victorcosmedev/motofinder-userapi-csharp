using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MotoFindrUserAPI.Infra.Data.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoFindrUserAPI.Infra.Data.HealthCheck
{
    public  class OracleHealthCheck : IHealthCheck
    {
        private readonly ApplicationContext _context;
        public OracleHealthCheck(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _context.Moto
                    .AsNoTracking()
                    .Take(1)
                    .AnyAsync(cancellationToken);

                return HealthCheckResult.Healthy("Banco de dados esta online");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Banco de dados esta offline", ex);
            }
        }
    }
}
