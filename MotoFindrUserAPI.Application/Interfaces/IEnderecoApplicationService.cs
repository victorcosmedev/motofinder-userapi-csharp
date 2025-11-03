using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Models.PageResultModel;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IEnderecoApplicationService
    {
        Task<OperationResult<EnderecoDto?>> ObterPorIdAsync(int id);
        Task<OperationResult<EnderecoDto>> CriarAsync(EnderecoDto endereco);
        Task<OperationResult<EnderecoDto?>> AtualizarAsync(int id, EnderecoDto endereco);
        Task<OperationResult<EnderecoDto?>> DeletarAsync(int id);
        Task<OperationResult<PageResultModel<IEnumerable<EnderecoDto?>>>> ObterTodos(int pageNumber = 1, int pageSize = 10);
    }
}
