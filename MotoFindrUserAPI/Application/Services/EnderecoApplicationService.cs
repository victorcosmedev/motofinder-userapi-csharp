using AutoMapper;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;

namespace MotoFindrUserAPI.Application.Services
{
    public class EnderecoApplicationService : IEnderecoApplicationService
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMotoqueiroRepository _motoqueiroRepository;
        private readonly IMapper _mapper;
        public EnderecoApplicationService(IEnderecoRepository enderecoRepository, IMotoqueiroRepository motoqueiroRepository, IMapper mapper)
        {
            _motoqueiroRepository = motoqueiroRepository;
            _enderecoRepository = enderecoRepository;
            _mapper = mapper;
        }

        public async Task<EnderecoDTO?> ObterPorIdAsync(int id)
        {
            var entity = await _enderecoRepository.BuscarPorIdAsync(id);
            return _mapper.Map<EnderecoDTO>(entity);
        }

        public async Task<EnderecoDTO> CriarAsync(EnderecoDTO endereco)
        {
            var entity = _mapper.Map<EnderecoEntity>(endereco);
            if (!endereco.MotoqueiroId.HasValue || endereco.MotoqueiroId == 0)
                throw new Exception("O ID do motoqueiro é obrigatório para criar um endereço.");

            var motoqueiro = await AtribuirEValidarMotoqueiroAsync(endereco.MotoqueiroId.Value, entity);
            entity.Motoqueiro = motoqueiro;
            entity.MotoqueiroId = motoqueiro.Id;

            entity = await _enderecoRepository.SalvarAsync(entity);
            return _mapper.Map<EnderecoDTO>(entity);
        }

        public async Task<bool> AtualizarAsync(int id, EnderecoDTO endereco)
        {
            var entity = _mapper.Map<EnderecoEntity>(endereco);
            var motoqueiro = await AtribuirEValidarMotoqueiroAsync(endereco.MotoqueiroId.Value, entity);
            
            entity.Motoqueiro = motoqueiro;
            entity.MotoqueiroId = motoqueiro.Id;

            return await _enderecoRepository.AtualizarAsync(id, entity);
        }

        public async Task<bool> DeletarAsync(int id)
        {
            return await _enderecoRepository.DeletarAsync(id);
        }

        private async Task<MotoqueiroEntity> AtribuirEValidarMotoqueiroAsync(int motoqueiroId, EnderecoEntity endereco)
        {
            var motoqueiro = await _motoqueiroRepository.BuscarPorIdAsync(motoqueiroId);
            if (motoqueiro == null)
                throw new Exception($"Motoqueiro de id {motoqueiroId} não existe.");

            if (motoqueiro.EnderecoId.HasValue && motoqueiro.EnderecoId != 0)
                throw new Exception($"Motoqueiro de id {motoqueiroId} já possui um endereço atribuído.");

            endereco.MotoqueiroId = motoqueiroId;
            return _mapper.Map<MotoqueiroEntity>(motoqueiro);
        }
    }
}
