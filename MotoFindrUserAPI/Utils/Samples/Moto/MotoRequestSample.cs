using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Moto
{
    public class MotoRequestSample : IExamplesProvider<MotoDTO>
    {
        public MotoDTO GetExamples()
        {
            return new MotoDTO
            {
                Id = 0, // Usually 0 for create requests
                Modelo = "Honda CB 500F",
                AnoDeFabricacao = 2022,
                Chassi = "9C2PC4000MR123456",
                Placa = "ABC1D23",
                MotoqueiroId = 1
            };
        }
    }
}
