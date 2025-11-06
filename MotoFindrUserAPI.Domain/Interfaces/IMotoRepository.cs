using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Models.PageResultModel;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IMotoRepository
    {
        Task<MotoEntity?> BuscarPorIdAsync(int id);
        Task<MotoEntity?> BuscarPorPlacaAsync(string placa);
        Task<MotoEntity?> BuscarPorChassiAsync(string chassi);
        Task<PageResultModel<IEnumerable<MotoEntity?>>> BuscarTodos(int pageNumber = 1, int pageSize = 10);
        Task<MotoEntity> SalvarAsync(MotoEntity moto);
        Task<bool> AtualizarAsync(int id, MotoEntity moto);
        Task<bool> DeletarAsync(int id);
        Task<IEnumerable<MotoEntity>> ObterMotosComPrecoParaTreinamento();
    }
}
