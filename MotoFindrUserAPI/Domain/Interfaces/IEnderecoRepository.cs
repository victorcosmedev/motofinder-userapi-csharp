using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IEnderecoRepository
    {
        Task<EnderecoEntity?> BuscarPorIdAsync(int id);
        Task<EnderecoEntity> SalvarAsync(EnderecoEntity endereco);
        Task<bool> AtualizarAsync(int id, EnderecoEntity endereco);
        Task<bool> DeletarAsync(int id);
    }
}
