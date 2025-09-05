using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Moto
{
    public class MotoResponseSample : IExamplesProvider<MotoDTO>
    {
        public MotoDTO GetExamples()
        {
            return new MotoDTO
            {
                Id = 1,
                Modelo = "Yamaha MT-07",
                AnoDeFabricacao = 2021,
                Chassi = "JYARM0611MA001234",
                Placa = "XYZ9E87",
                MotoqueiroId = 2
            };
        }
    }
}
