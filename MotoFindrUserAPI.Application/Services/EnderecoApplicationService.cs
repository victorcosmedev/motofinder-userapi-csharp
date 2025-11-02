using AutoMapper;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Domain.Models.PageResultModel;
using System.Net;

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

        public async Task<OperationResult<EnderecoDto?>> ObterPorIdAsync(int id)
        {
            try
            {
                var entity = await _enderecoRepository.BuscarPorIdAsync(id);
                if (entity == null)
                    return OperationResult<EnderecoDto?>.Failure($"Endereço de id {id} não encontrado.", (int)HttpStatusCode.NotFound);
                return OperationResult<EnderecoDto?>.Success(_mapper.Map<EnderecoDto>(entity));
            }
            catch(Exception ex)
            {
                return OperationResult<EnderecoDto?>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
            }

        }

        public async Task<OperationResult<PageResultModel<IEnumerable<EnderecoDto?>>>> ObterTodos(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var pageResult = await _enderecoRepository.BuscarTodos(pageNumber, pageSize);
                if (pageResult == null || !pageResult.Items.Any()) return OperationResult<PageResultModel<IEnumerable<EnderecoDto?>>>.Failure("Nenhum endereço encontrado.", (int)HttpStatusCode.NotFound);

                var dtos = pageResult.Items.Select(x => _mapper.Map<EnderecoDto>(x));

                var pageResultDto = new PageResultModel<IEnumerable<EnderecoDto?>>
                {
                    Items = dtos,
                    TotalItens = pageResult.TotalItens,
                    NumeroPagina = pageResult.NumeroPagina,
                    TamanhoPagina = pageResult.TamanhoPagina
                };

                return OperationResult<PageResultModel<IEnumerable<EnderecoDto?>>>.Success(pageResultDto);
            }
            catch(Exception ex)
            {
                return OperationResult<PageResultModel<IEnumerable<EnderecoDto?>>>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<OperationResult<EnderecoDto?>> CriarAsync(EnderecoDto endereco)
        {
            try
            {
                var entity = _mapper.Map<EnderecoEntity>(endereco);
                if (!endereco.MotoqueiroId.HasValue || endereco.MotoqueiroId == 0)
                    throw new Exception("O ID do motoqueiro é obrigatório para criar um endereço.");

                var motoqueiro = await AtribuirEValidarMotoqueiroAsync(endereco.MotoqueiroId.Value, entity);

                entity = await _enderecoRepository.SalvarAsync(entity);
                motoqueiro.Endereco = entity;
                motoqueiro.EnderecoId = entity.Id;

                await _motoqueiroRepository.AtualizarAsync(motoqueiro.Id, motoqueiro);

                return OperationResult<EnderecoDto?>.Success(_mapper.Map<EnderecoDto>(entity));
            }
            catch (Exception ex)
            {
                return OperationResult<EnderecoDto?>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<OperationResult<EnderecoDto?>> AtualizarAsync(int id, EnderecoDto endereco)
        {
            try
            {
                var entity = _mapper.Map<EnderecoEntity>(endereco);
                var motoqueiro = await AtribuirEValidarMotoqueiroAsync(endereco.MotoqueiroId.Value, entity);

                var success = await _enderecoRepository.AtualizarAsync(id, entity);

                if(success) return OperationResult<EnderecoDto?>.Success(null);

                return OperationResult<EnderecoDto?>.Failure("Endereço não encontrado para atualização.", (int)HttpStatusCode.NotFound);
            }
            catch (Exception)
            {
                return OperationResult<EnderecoDto?>.Failure("Erro ao atualizar o endereço.", (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<OperationResult<EnderecoDto?>> DeletarAsync(int id)
        {
            try
            {
                var success = await _enderecoRepository.DeletarAsync(id);
                if(success) return OperationResult<EnderecoDto?>.Success(null);

                return OperationResult<EnderecoDto?>.Failure("Endereço não encontrado para deleção.", (int)HttpStatusCode.NotFound);
            }
            catch(Exception ex)
            {
                return OperationResult<EnderecoDto?>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
            }
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
