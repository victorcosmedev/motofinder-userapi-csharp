using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Motoqueiro
{
    public class MotoqueiroResponseSample : IExamplesProvider<MotoqueiroDTO>
    {
        public MotoqueiroDTO GetExamples()
        {
            return new MotoqueiroDTO
            {
                Id = 1,
                Nome = "Barbara Oliveira",
                Cpf = "98765432100",
                DataNascimento = new DateTime(1990, 3, 15),
                Endereco = new EnderecoDto
                {
                    Id = 1,
                    Logradouro = "Av. Paulista",
                    Numero = "1000",
                    Complemento = "Conj. 101",
                    Uf = "SP",
                    Municipio = "São Paulo",
                    Cep = "01310200",
                    MotoqueiroId = 1
                },
                MotoId = 2
            };
        }
    }
}
