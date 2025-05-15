using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IMotoqueiroRepository
    {
        Task<MotoqueiroEntity?> BuscarPorIdAsync(int id);
        Task<MotoqueiroEntity?> BuscarPorCpfAsync(string cpf);
        Task<IEnumerable<MotoqueiroEntity?>> BuscarTodos();
        Task<MotoqueiroEntity> SalvarAsync(MotoqueiroEntity motoqueiro);
        Task<bool> AtualizarAsync(int id, MotoqueiroEntity motoqueiro);
        Task<bool> DeletarAsync(int id);
    }
}
