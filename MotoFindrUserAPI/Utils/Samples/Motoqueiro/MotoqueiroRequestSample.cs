using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Motoqueiro
{
    public class MotoqueiroRequestSample : IExamplesProvider<MotoqueiroDTO>
    {
        public MotoqueiroDTO GetExamples()
        {
            return new MotoqueiroDTO
            {
                Id = 0,
                Nome = "Carlos Silva",
                Cpf = "12345678901",
                DataNascimento = new DateTime(1995, 5, 20),
                EnderecoId = 0,
                Endereco = null,
                MotoId = 0
            };
        }
    }
}
