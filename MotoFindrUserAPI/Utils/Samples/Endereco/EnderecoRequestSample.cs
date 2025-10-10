using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Endereco
{
    public class EnderecoRequestSample : IExamplesProvider<EnderecoDto>
    {
        public EnderecoDto GetExamples()
        {
            return new EnderecoDto
            {
                Id = 0,
                Logradouro = "Rua Central",
                Numero = "55",
                Complemento = "Casa",
                Uf = "SP",
                Municipio = "Campinas",
                Cep = "13040000",
                MotoqueiroId = 1
            };
        }
    }
}
