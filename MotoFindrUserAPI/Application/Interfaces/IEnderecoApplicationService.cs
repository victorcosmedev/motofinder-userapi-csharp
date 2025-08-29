using MotoFindrUserAPI.Application.DTOs;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IEnderecoApplicationService
    {
        Task<EnderecoDTO?> ObterPorIdAsync(int id);
        Task<EnderecoDTO> CriarAsync(EnderecoDTO endereco);
        Task<bool> AtualizarAsync(int id, EnderecoDTO endereco);
        Task<bool> DeletarAsync(int id);
    }
}
