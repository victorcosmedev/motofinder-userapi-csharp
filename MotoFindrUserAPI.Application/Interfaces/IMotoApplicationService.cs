using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Models.PageResultModel;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IMotoApplicationService
    {
        Task<OperationResult<MotoDto?>> ObterPorIdAsync(int id);
        Task<OperationResult<MotoDto?>> ObterPorPlacaAsync(string placa);
        Task<OperationResult<MotoDto?>> ObterPorChassiAsync(string chassi);
        Task<OperationResult<PageResultModel<IEnumerable<MotoDto?>>>> ObterTodos(int pageNumber = 1, int pageSize = 10);
        Task<OperationResult<MotoDto?>> CriarAsync(MotoDto moto);
        Task<OperationResult<MotoDto?>> AtualizarAsync(int id, MotoDto moto);
        Task<OperationResult<MotoDto?>> RemoverAsync(int id);
    }
}
