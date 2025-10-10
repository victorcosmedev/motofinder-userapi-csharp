using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Models.PageResultModel;

namespace MotoFindrUserAPI.Application.Interfaces
{
    public interface IMotoApplicationService
    {
        Task<MotoDto?> ObterPorIdAsync(int id);
        Task<MotoDto?> ObterPorPlacaAsync(string placa);
        Task<MotoDto?> ObterPorChassiAsync(string chassi);
        Task<PageResultModel<IEnumerable<MotoDto?>>> ObterTodos(int pageNumber = 1, int pageSize = 10);
        Task<MotoDto> CriarAsync(MotoDto moto);
        Task<bool> AtualizarAsync(int id, MotoDto moto);
        Task<bool> RemoverAsync(int id);
    }
}
