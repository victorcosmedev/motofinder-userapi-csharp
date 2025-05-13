using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IMotoqueiroApplicationService
    {
        Task<MotoqueiroEntity?> ObterPorIdAsync(int id);
        Task<MotoqueiroEntity?> ObterPorCpfAsync(string cpf);
        Task<MotoqueiroEntity> CriarAsync(MotoqueiroEntity motoqueiro);
        Task<bool> AtualizarAsync(int id, MotoqueiroEntity motoqueiro);
        Task<bool> RemoverAsync(int id);
    }
}
