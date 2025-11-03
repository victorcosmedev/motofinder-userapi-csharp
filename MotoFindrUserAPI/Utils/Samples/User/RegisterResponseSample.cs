using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.User
{
    public class RegisterResponseSample : IExamplesProvider<MessageResponseDto>
    {
        public MessageResponseDto GetExamples()
        {
            return new MessageResponseDto
            {
                Message = "Usuário criado com sucesso!"
            };
        }
    }
}
