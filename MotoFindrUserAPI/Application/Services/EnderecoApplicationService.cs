using AutoMapper;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Models.Hateoas;
using MotoFindrUserAPI.Models.PageResultModel;

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

        public async Task<EnderecoDto?> ObterPorIdAsync(int id)
        {
            var entity = await _enderecoRepository.BuscarPorIdAsync(id);
            return _mapper.Map<EnderecoDto>(entity);
        }

        public async Task<PageResultModel<IEnumerable<EnderecoDto?>>> ObterTodos(int pageNumber = 1, int pageSize = 10)
        {
            var pageResult = await _enderecoRepository.BuscarTodos(pageNumber, pageSize);

            var dtos = pageResult.Items.Select(x => _mapper.Map<EnderecoDto>(x));

            var pageResultDto = new PageResultModel<IEnumerable<EnderecoDto?>>
            {
                Items = dtos,
                TotalItens = pageResult.TotalItens,
                NumeroPagina = pageResult.NumeroPagina,
                TamanhoPagina = pageResult.TamanhoPagina
            };

            return pageResultDto;
        }

        public async Task<EnderecoDto> CriarAsync(EnderecoDto endereco)
        {
            var entity = _mapper.Map<EnderecoEntity>(endereco);
            if (!endereco.MotoqueiroId.HasValue || endereco.MotoqueiroId == 0)
                throw new Exception("O ID do motoqueiro é obrigatório para criar um endereço.");

            var motoqueiro = await AtribuirEValidarMotoqueiroAsync(endereco.MotoqueiroId.Value, entity);

            entity = await _enderecoRepository.SalvarAsync(entity);
            motoqueiro.Endereco = entity;
            motoqueiro.EnderecoId = entity.Id;

            await _motoqueiroRepository.AtualizarAsync(motoqueiro.Id, motoqueiro);

            return _mapper.Map<EnderecoDto>(entity);
        }

        public async Task<bool> AtualizarAsync(int id, EnderecoDto endereco)
        {
            var entity = _mapper.Map<EnderecoEntity>(endereco);
            var motoqueiro = await AtribuirEValidarMotoqueiroAsync(endereco.MotoqueiroId.Value, entity);

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

            if (motoqueiro.EnderecoId.HasValue && motoqueiro.EnderecoId != 0 && motoqueiro.EnderecoId != endereco.Id)
                throw new Exception($"Motoqueiro de id {motoqueiroId} já possui um endereço atribuído.");

            endereco.MotoqueiroId = motoqueiroId;
            endereco.Motoqueiro = motoqueiro;

            motoqueiro.Endereco = endereco;
            motoqueiro.EnderecoId = endereco.Id;

            await _motoqueiroRepository.AtualizarAsync(motoqueiroId, motoqueiro);

            return _mapper.Map<MotoqueiroEntity>(motoqueiro);
        }


    }
}
