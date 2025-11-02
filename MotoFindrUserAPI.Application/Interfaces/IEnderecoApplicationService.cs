using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Models.PageResultModel;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IEnderecoApplicationService
    {
        Task<EnderecoDto?> ObterPorIdAsync(int id);
        Task<EnderecoDto> CriarAsync(EnderecoDto endereco);
        Task<bool> AtualizarAsync(int id, EnderecoDto endereco);
        Task<bool> DeletarAsync(int id);
        Task<PageResultModel<IEnumerable<EnderecoDto?>>> ObterTodos(int pageNumber = 1, int pageSize = 10);
    }
}
