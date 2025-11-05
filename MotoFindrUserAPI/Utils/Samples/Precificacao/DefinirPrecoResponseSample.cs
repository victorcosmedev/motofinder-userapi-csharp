using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Precificacao
{
    public class DefinirPrecoResponseSample : IExamplesProvider<PrecificacaoMotoDto>
    {
        public PrecificacaoMotoDto GetExamples()
        {
            return new PrecificacaoMotoDto
            {
                Id = 1,
                MotoId = 10,
                Preco = 15000.00
            };
        }
    }
}
