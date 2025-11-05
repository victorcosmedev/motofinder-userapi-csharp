using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Precificacao
{
    public class PreverPrecoResponseSample : IExamplesProvider<PrevisaoPrecoDto>
    {
        public PrevisaoPrecoDto GetExamples(){
            return new PrevisaoPrecoDto
            {
                Modelo = "Honda CB 500F",
                AnoDeFabricacao = 2022,
                PrecoEstimado = 34500.75
            };
        }
    }
}
