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
        private readonly IMapper _mapper;
        public EnderecoApplicationService(IEnderecoRepository enderecoRepository, IMapper mapper)
        {
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
            entity = await _enderecoRepository.SalvarAsync(entity);
            return _mapper.Map<EnderecoDTO>(entity);
        }

        public async Task<bool> AtualizarAsync(int id, EnderecoDTO endereco)
        {
            var entity = _mapper.Map<EnderecoEntity>(endereco);
            return await _enderecoRepository.AtualizarAsync(id, entity);
        }

        public async Task<bool> DeletarAsync(int id)
        {
            return await _enderecoRepository.DeletarAsync(id);
        }
    }
}
