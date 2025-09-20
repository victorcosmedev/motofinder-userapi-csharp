using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Models.PageResultModel;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IMotoqueiroApplicationService
    {
        Task<MotoqueiroDTO?> ObterPorIdAsync(int id);
        Task<MotoqueiroDTO?> ObterPorCpfAsync(string cpf);
        Task<PageResultModel<IEnumerable<MotoqueiroDTO?>>> ObterTodos(int pageNumber = 1, int pageSize = 10);
        Task<MotoqueiroDTO> CriarAsync(MotoqueiroDTO motoqueiro);
        Task<bool> AtualizarAsync(int id, MotoqueiroDTO motoqueiro);
        Task<bool> RemoverAsync(int id);
    }
}
