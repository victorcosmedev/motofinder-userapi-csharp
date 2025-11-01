using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Models.PageResultModel;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IMotoqueiroApplicationService
    {
        Task<MotoqueiroDto?> ObterPorIdAsync(int id);
        Task<MotoqueiroDto?> ObterPorCpfAsync(string cpf);
        Task<PageResultModel<IEnumerable<MotoqueiroDto?>>> ObterTodos(int pageNumber = 1, int pageSize = 10);
        Task<MotoqueiroDto> CriarAsync(MotoqueiroDto motoqueiro);
        Task<bool> AtualizarAsync(int id, MotoqueiroDto motoqueiro);
        Task<bool> RemoverAsync(int id);
    }
}
