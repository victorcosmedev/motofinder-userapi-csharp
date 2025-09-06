using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Models.PageResultModel;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IMotoApplicationService
    {
        Task<MotoDTO?> ObterPorIdAsync(int id);
        Task<MotoDTO?> ObterPorPlacaAsync(string placa);
        Task<MotoDTO?> ObterPorChassiAsync(string chassi);
        Task<PageResultModel<IEnumerable<MotoDTO?>>> ObterTodos(int pageNumber, int pageSize);
        Task<MotoDTO> CriarAsync(MotoDTO moto);
        Task<bool> AtualizarAsync(int id, MotoDTO moto);
        Task<bool> RemoverAsync(int id);
    }
}
