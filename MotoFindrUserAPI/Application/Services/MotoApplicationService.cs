using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;

namespace MotoFindrUserAPI.Application.Services
{
    public class MotoApplicationService : IMotoApplicationService
    {
        private readonly IMotoRepository _motoRepository;
        private readonly IMotoqueiroRepository _motoqueiroRepository;

        public MotoApplicationService(IMotoRepository motoRepository, IMotoqueiroRepository motoqueiroRepository)
        {
            _motoRepository = motoRepository;
            _motoqueiroRepository = motoqueiroRepository;
        }

        public async Task<MotoEntity?> ObterPorIdAsync(int id)
        {
            return await _motoRepository.BuscarPorIdAsync(id);
        }

        public async Task<MotoEntity?> ObterPorPlacaAsync(string placa)
        {
            return await _motoRepository.BuscarPorPlacaAsync(placa);
        }

        public async Task<MotoEntity?> ObterPorChassiAsync(string chassi)
        {
            return await _motoRepository.BuscarPorChassiAsync(chassi);
        }

        public async Task<MotoEntity> CriarAsync(MotoEntity moto)
        {
            if (moto.MotoqueiroId.HasValue)
            {
                var motoqueiro = await _motoqueiroRepository.BuscarPorIdAsync(moto.MotoqueiroId.Value);
                if (motoqueiro == null)
                    throw new Exception("Motoqueiro não encontrado.");

                if (motoqueiro.Moto != null && motoqueiro.Moto.Id != moto.Id)
                    throw new Exception("Este motoqueiro já possui uma moto cadastrada.");
            }

            return await _motoRepository.SalvarAsync(moto);
        }

        public async Task<bool> AtualizarAsync(int id, MotoEntity moto)
        {
            return await _motoRepository.AtualizarAsync(id, moto);
        }

        public async Task<bool> RemoverAsync(int id)
        {
            return await _motoRepository.DeletarAsync(id);
        }
    }
}
