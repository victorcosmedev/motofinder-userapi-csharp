using AutoMapper;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;

namespace MotoFindrUserAPI.Application.Services
{
    public class PrecificacaoMotoApplicationService : IPrecificacaoMotoApplicationService
    {
        private readonly IPrecificacaoMotoRepository _repository;
        private readonly IMotoRepository _motoRepository;
        private readonly IMapper _mapper;

        public PrecificacaoMotoApplicationService(IPrecificacaoMotoRepository repository, IMotoRepository motoRepository, IMapper mapper)
        {
            _repository = repository;
            _motoRepository = motoRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<PrecificacaoMotoDto>> DefinirPrecoMotoAsync(int motoId, double preco)
        {
            var moto = await _motoRepository.BuscarPorIdAsync(motoId);
            if (moto == null)
                return OperationResult<PrecificacaoMotoDto>.Failure("Moto não encontrada");

            var precificacao = new PrecificacaoMotoEntity
            {
                MotoId = motoId,
                Preco = preco
            };

            var entidadeSalva = await _repository.SalvarOuAtualizarAsync(precificacao);

            var dto = _mapper.Map<PrecificacaoMotoDto>(entidadeSalva);
            return OperationResult<PrecificacaoMotoDto>.Success(dto);
        }

        public IEnumerable<PrecificacaoTreinamentoDto> ObterDadosTreinamento()
        {
            var entidades = _repository.ObterDadosParaTreinamento();

            return _mapper.Map<IEnumerable<PrecificacaoTreinamentoDto>>(entidades);
        }
    }
}
