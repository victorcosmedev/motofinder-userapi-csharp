using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IMotoRepository
    {
        Task<MotoEntity?> BuscarPorIdAsync(int id);
        Task<MotoEntity?> BuscarPorPlacaAsync(string placa);
        Task<MotoEntity?> BuscarPorChassiAsync(string chassi);
        Task<IEnumerable<MotoEntity?>> BuscarTodos();
        Task<MotoEntity> SalvarAsync(MotoEntity moto);
        Task<bool> AtualizarAsync(int id, MotoEntity moto);
        Task<bool> DeletarAsync(int id);
    }
}
