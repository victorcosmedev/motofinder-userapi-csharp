using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoFindrUserAPI.Application.DTOs
{
    public class PrecificacaoTreinamentoDto
    {
        public string Modelo { get; set; } = string.Empty;
        public int AnoDeFabricacao { get; set; }
        public double Preco { get; set; }
    }
}
