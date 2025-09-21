using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Models.PageResultModel;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IEnderecoApplicationService
    {
        Task<EnderecoDTO?> ObterPorIdAsync(int id);
        Task<EnderecoDTO> CriarAsync(EnderecoDTO endereco);
        Task<bool> AtualizarAsync(int id, EnderecoDTO endereco);
        Task<bool> DeletarAsync(int id);
        Task<PageResultModel<IEnumerable<EnderecoDTO?>>> ObterTodos(int pageNumber = 1, int pageSize = 10);
    }
}
