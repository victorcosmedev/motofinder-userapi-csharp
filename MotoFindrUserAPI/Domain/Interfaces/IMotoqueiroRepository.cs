using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Models.PageResultModel;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IMotoqueiroRepository
    {
        Task<MotoqueiroEntity?> BuscarPorIdAsync(int id);
        Task<MotoqueiroEntity?> BuscarPorCpfAsync(string cpf);
        Task<PageResultModel<IEnumerable<MotoqueiroEntity?>>> BuscarTodos(int pageNumber, int pageSize);
        Task<MotoqueiroEntity> SalvarAsync(MotoqueiroEntity motoqueiro);
        Task<bool> AtualizarAsync(int id, MotoqueiroEntity motoqueiro);
        Task<bool> DeletarAsync(int id);
    }
}
