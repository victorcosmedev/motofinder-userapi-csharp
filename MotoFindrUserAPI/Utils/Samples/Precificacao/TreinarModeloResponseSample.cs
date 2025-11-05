using MotoFindrUserAPI.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace MotoFindrUserAPI.Utils.Samples.Precificacao
{
    public class TreinarModeloResponseSample : IExamplesProvider<MessageResponseDto>
    {
        public MessageResponseDto GetExamples()
        {
            return new MessageResponseDto
            {
                Message = "Modelo treinado."
            };
        }
    }
}
