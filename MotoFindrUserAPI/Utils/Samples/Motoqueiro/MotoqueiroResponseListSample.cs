using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Motoqueiro
{
    public class MotoqueiroResponseListSample : IExamplesProvider<IEnumerable<MotoqueiroDto>>
    {
        public IEnumerable<MotoqueiroDto> GetExamples()
        {
            return new List<MotoqueiroDto>
        {
            new MotoqueiroDto {
                Id = 1,
                Nome = "Pedro Santos",
                Cpf = "11122233344",
                DataNascimento = new DateTime(1988, 1, 1),
                Endereco = new EnderecoDto {
                    Id = 1,
                    Logradouro = "Rua A",
                    Numero = "12",
                    Complemento = "Casa",
                    Uf = "RJ",
                    Municipio = "Rio de Janeiro",
                    Cep = "20010000",
                    MotoqueiroId = 1
                },
                MotoId = 1
            },
            new MotoqueiroDto {
                Id = 2,
                Nome = "Maria Costa",
                Cpf = "55566677788",
                DataNascimento = new DateTime(1992, 12, 25),
                Endereco = new EnderecoDto {
                    Id = 2,
                    Logradouro = "Rua B",
                    Numero = "45",
                    Complemento = "Bloco 2",
                    Uf = "MG",
                    Municipio = "Belo Horizonte",
                    Cep = "30110000",
                    MotoqueiroId = 2
                },
                MotoId = 2
            }
        };
        }
    }
}
