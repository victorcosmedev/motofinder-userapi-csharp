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
            var entity = _mapper.Map<MotoqueiroEntity>(motoqueiro);
            if (motoqueiro.MotoId.HasValue && motoqueiro.MotoId != 0)
            {
                moto = await AtribuirEValidarMotoAsync(motoqueiro.MotoId.Value, entity);
            }

            entity.Moto = moto;

            entity = await _motoqueiroRepository.SalvarAsync(entity);
            return _mapper.Map<MotoqueiroDTO>(entity);
        }

        public async Task<bool> AtualizarAsync(int id, MotoqueiroDTO motoqueiro)
        {
            MotoEntity? moto = null;
            var entity = _mapper.Map<MotoqueiroEntity>(motoqueiro);
            if (motoqueiro.MotoId.HasValue && motoqueiro.MotoId != 0)
            {
                moto = await AtribuirEValidarMotoAsync(motoqueiro.MotoId.Value, entity);
            }
            entity.Moto = moto;
            return await _motoqueiroRepository.AtualizarAsync(id, entity);
        }

        public async Task<bool> RemoverAsync(int id)
        {
            return await _motoqueiroRepository.DeletarAsync(id);
        }

        private async Task<MotoEntity?> AtribuirEValidarMotoAsync(int motoId, MotoqueiroEntity motoqueiroEntity)
        {
            var moto = await _motoRepository.BuscarPorIdAsync(motoId);
            if (moto == null)
                throw new Exception("Moto não encontrada");

            if (moto.Motoqueiro != null && moto.Motoqueiro.Id != motoId)
                throw new Exception("Esta moto já está associada a outro motoqueiro.");

            moto.Motoqueiro = motoqueiroEntity;
            moto.MotoqueiroId = motoqueiroEntity.MotoId;

            await _motoRepository.AtualizarAsync(motoId, moto);
            return moto;
        }

        public async Task<IEnumerable<MotoqueiroDTO?>> ObterTodos()
        {
            var entities = await _motoqueiroRepository.BuscarTodos();
            return entities.Select(x => _mapper.Map<MotoqueiroDTO>(x));
        }
    }
}

