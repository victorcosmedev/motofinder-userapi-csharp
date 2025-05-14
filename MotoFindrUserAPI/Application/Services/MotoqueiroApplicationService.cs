using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Domain.Interfaces;
using AutoMapper;
using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Application.Services
{
    public class MotoqueiroApplicationService : IMotoqueiroApplicationService
    {
        private readonly IMotoqueiroRepository _motoqueiroRepository;
        private readonly IMotoRepository _motoRepository;
        private readonly IMapper _mapper;

        public MotoqueiroApplicationService(IMotoqueiroRepository motoqueiroRepository, IMotoRepository motoRepository, IMapper mapper)
        {
            _motoqueiroRepository = motoqueiroRepository;
            _motoRepository = motoRepository;
            _mapper = mapper;
        }

        public async Task<MotoqueiroDTO?> ObterPorIdAsync(int id)
        {
            var entity = await _motoqueiroRepository.BuscarPorIdAsync(id);
            return _mapper.Map<MotoqueiroDTO>(entity);
        }

        public async Task<MotoqueiroDTO?> ObterPorCpfAsync(string cpf)
        {
            var entity = await _motoqueiroRepository.BuscarPorCpfAsync(cpf);
            return _mapper.Map<MotoqueiroDTO>(entity);
        }

        public async Task<MotoqueiroDTO> CriarAsync(MotoqueiroDTO motoqueiro)
        {
            MotoEntity? moto = null;
            if (motoqueiro.MotoId.HasValue)
            {
                moto = await AtribuirEValidarMoto(motoqueiro.MotoId.Value);
            }

            var entity = _mapper.Map<MotoqueiroEntity>(motoqueiro);
            entity.Moto = moto;

            entity = await _motoqueiroRepository.SalvarAsync(entity);
            return _mapper.Map<MotoqueiroDTO>(entity);
        }

        public async Task<bool> AtualizarAsync(int id, MotoqueiroDTO motoqueiro)
        {
            return await _motoqueiroRepository.AtualizarAsync(id, motoqueiro);
        }

        public async Task<bool> RemoverAsync(int id)
        {
            return await _motoqueiroRepository.DeletarAsync(id);
        }

        private async Task<MotoEntity?> AtribuirEValidarMoto(int id)
        {
            var moto = await _motoRepository.BuscarPorIdAsync(id);
            if (moto == null)
                throw new Exception("Moto não encontrada");

            if (moto.Motoqueiro != null && moto.Motoqueiro.Id != id)
                throw new Exception("Esta moto já está associada a outro motoqueiro.");
            return moto;
        }
    }
}

