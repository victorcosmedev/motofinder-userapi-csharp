using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Endereco
{
    public class EnderecoResponseListSample : IExamplesProvider<IEnumerable<EnderecoDto>>
    {
        public IEnumerable<EnderecoDto> GetExamples()
        {
            return new List<EnderecoDto>
            {
                new EnderecoDto {
                    Id = 1,
                    Logradouro = "Av. Brasil",
                    Numero = "10",
                    Complemento = "Apto 101",
                    Uf = "RJ",
                    Municipio = "Niterói",
                    Cep = "24020000",
                    MotoqueiroId = 1
                },
                new EnderecoDto {
                    Id = 2,
                    Logradouro = "Rua das Palmeiras",
                    Numero = "20",
                    Complemento = "Casa",
                    Uf = "BA",
                    Municipio = "Salvador",
                    Cep = "40010000",
                    MotoqueiroId = 2
                }
            };
        }
    }
}
