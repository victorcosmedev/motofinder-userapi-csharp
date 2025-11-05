using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IPrecificacaoMotoApplicationService
    {
        Task<OperationResult<PrecificacaoMotoDto>> DefinirPrecoMotoAsync(int motoId, double preco);
        IEnumerable<PrecificacaoTreinamentoDto> ObterDadosTreinamento();
    }
}
