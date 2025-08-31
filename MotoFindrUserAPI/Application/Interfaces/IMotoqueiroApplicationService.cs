using MotoFindrUserAPI.Application.DTOs;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IMotoqueiroApplicationService
    {
        Task<MotoqueiroDTO?> ObterPorIdAsync(int id);
        Task<MotoqueiroDTO?> ObterPorCpfAsync(string cpf);
        Task<IEnumerable<MotoqueiroDTO?>> ObterTodos(int pageNumber, int pageSize);
        Task<MotoqueiroDTO> CriarAsync(MotoqueiroDTO motoqueiro);
        Task<bool> AtualizarAsync(int id, MotoqueiroDTO motoqueiro);
        Task<bool> RemoverAsync(int id);
    }
}
