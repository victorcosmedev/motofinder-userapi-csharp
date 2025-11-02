using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Models.PageResultModel;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IEnderecoRepository
    {
        Task<EnderecoEntity?> BuscarPorIdAsync(int id);
        Task<EnderecoEntity> SalvarAsync(EnderecoEntity endereco);
        Task<bool> AtualizarAsync(int id, EnderecoEntity endereco);
        Task<bool> DeletarAsync(int id);
        Task<PageResultModel<IEnumerable<EnderecoEntity?>>> BuscarTodos(int pageNumber = 1, int pageSize = 10);
    }
}
