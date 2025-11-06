using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Models.PageResultModel;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IMotoqueiroRepository
    {
        Task<MotoqueiroEntity?> BuscarPorIdAsync(int id);
        Task<MotoqueiroEntity?> BuscarPorCpfAsync(string cpf);
        Task<PageResultModel<IEnumerable<MotoqueiroEntity?>>> BuscarTodos(int pageNumber = 1, int pageSize = 10);
        Task<MotoqueiroEntity> SalvarAsync(MotoqueiroEntity motoqueiro);
        Task<bool> AtualizarAsync(int id, MotoqueiroEntity motoqueiro);
        Task<bool> DeletarAsync(int id);
    }
}
