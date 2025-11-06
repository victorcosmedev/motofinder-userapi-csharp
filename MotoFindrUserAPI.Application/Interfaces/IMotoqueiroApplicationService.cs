using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Models.PageResultModel;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IMotoqueiroApplicationService
    {
        Task<OperationResult<MotoqueiroDto?>> ObterPorIdAsync(int id);
        Task<OperationResult<MotoqueiroDto?>> ObterPorCpfAsync(string cpf);
        Task<OperationResult<PageResultModel<IEnumerable<MotoqueiroDto?>>>> ObterTodos(int pageNumber = 1, int pageSize = 10);
        Task<OperationResult<MotoqueiroDto?>> CriarAsync(MotoqueiroDto motoqueiro);
        Task<OperationResult<MotoqueiroDto?>> AtualizarAsync(int id, MotoqueiroDto motoqueiro);
        Task<OperationResult<MotoqueiroDto?>> RemoverAsync(int id);
    }
}
