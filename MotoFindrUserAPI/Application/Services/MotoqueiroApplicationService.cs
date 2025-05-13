using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;

namespace MotoFindrUserAPI.Application.Services
{
    public class MotoqueiroApplicationService : IMotoqueiroApplicationService
    {
        private readonly IMotoqueiroRepository _motoqueiroRepository;

        public MotoqueiroApplicationService(IMotoqueiroRepository motoqueiroRepository)
        {
            _motoqueiroRepository = motoqueiroRepository;
        }

        public async Task<MotoqueiroEntity?> ObterPorIdAsync(int id)
        {
            return await _motoqueiroRepository.BuscarPorIdAsync(id);
        }

        public async Task<MotoqueiroEntity?> ObterPorCpfAsync(string cpf)
        {
            return await _motoqueiroRepository.BuscarPorCpfAsync(cpf);
        }

        public async Task<MotoqueiroEntity> CriarAsync(MotoqueiroEntity motoqueiro)
        {
            return await _motoqueiroRepository.SalvarAsync(motoqueiro);
        }

        public async Task<bool> AtualizarAsync(int id, MotoqueiroEntity motoqueiro)
        {
            return await _motoqueiroRepository.AtualizarAsync(id, motoqueiro);
        }

        public async Task<bool> RemoverAsync(int id)
        {
            return await _motoqueiroRepository.DeletarAsync(id);
        }
    }
}

