using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoFindrUserAPI.Application.DTOs
{
    public class HealthCheckItemDto
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Error { get; set; }
    }
}
