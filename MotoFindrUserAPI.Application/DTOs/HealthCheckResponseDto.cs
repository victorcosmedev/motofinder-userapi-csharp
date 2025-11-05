using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoFindrUserAPI.Application.DTOs
{
    public class HealthCheckResponseDto
    {
        public string Status { get; set; }
        public IEnumerable<HealthCheckItemDto> Checks { get; set; }
    }
}
