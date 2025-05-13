using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IMotoApplicationService
    {
        Task<MotoEntity?> ObterPorIdAsync(int id);
        Task<MotoEntity?> ObterPorPlacaAsync(string placa);
        Task<MotoEntity?> ObterPorChassiAsync(string chassi);
        Task<MotoEntity> CriarAsync(MotoEntity moto);
        Task<bool> AtualizarAsync(int id, MotoEntity moto);
        Task<bool> RemoverAsync(int id);
    }
}
