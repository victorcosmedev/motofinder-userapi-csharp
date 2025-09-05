using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Moto
{
    public class MotoResponseListSample : IExamplesProvider<IEnumerable<MotoDTO>>
    {
        public IEnumerable<MotoDTO> GetExamples()
        {
            return new List<MotoDTO>
        {
            new MotoDTO {
                Id = 1,
                Modelo = "Kawasaki Ninja 400",
                AnoDeFabricacao = 2020,
                Chassi = "JKAZX400HNA123456",
                Placa = "AAA0B11",
                MotoqueiroId = 1
            },
            new MotoDTO {
                Id = 2,
                Modelo = "Suzuki GSX-S750",
                AnoDeFabricacao = 2019,
                Chassi = "JS1C533B3K7101234",
                Placa = "BBB2C33",
                MotoqueiroId = 2
            }
        };
        }
    }
}
