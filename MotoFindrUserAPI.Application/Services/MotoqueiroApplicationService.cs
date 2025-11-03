using AutoMapper;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Domain.Models.PageResultModel;
using System.Net;

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

        public async Task<OperationResult<MotoqueiroDto?>> ObterPorIdAsync(int id)
        {
            var entity = await _motoqueiroRepository.BuscarPorIdAsync(id);
            var dtos = _mapper.Map<MotoqueiroDto>(entity);
            return OperationResult<MotoqueiroDto?>.Success(dtos);
        }

        public async Task<OperationResult<MotoqueiroDto?>> ObterPorCpfAsync(string cpf)
        {
            var entity = await _motoqueiroRepository.BuscarPorCpfAsync(cpf);
            var dtos = _mapper.Map<MotoqueiroDto>(entity);
            return OperationResult<MotoqueiroDto?>.Success(dtos);
        }

        public async Task<OperationResult<MotoqueiroDto?>> CriarAsync(MotoqueiroDto motoqueiro)
        {
            MotoEntity? moto = null;
            var entity = _mapper.Map<MotoqueiroEntity>(motoqueiro);
            if (motoqueiro.MotoId.HasValue && motoqueiro.MotoId != 0)
            {
                moto = await AtribuirEValidarMotoAsync(motoqueiro.MotoId.Value, entity);
            }
            else
            {
                entity.MotoId = null;
            }

            entity.Moto = moto;

            entity = await _motoqueiroRepository.SalvarAsync(entity);
            return OperationResult<MotoqueiroDto?>.Success(_mapper.Map<MotoqueiroDto>(entity));
        }

        public async Task<OperationResult<MotoqueiroDto?>> AtualizarAsync(int id, MotoqueiroDto motoqueiro)
        {
            MotoEntity? moto = null;
            var entity = _mapper.Map<MotoqueiroEntity>(motoqueiro);
            if (motoqueiro.MotoId.HasValue && motoqueiro.MotoId != 0)
            {
                moto = await AtribuirEValidarMotoAsync(motoqueiro.MotoId.Value, entity);
            } else
            {
                entity.MotoId = null;
            }

            entity.Moto = moto;
            
            var success = await _motoqueiroRepository.AtualizarAsync(id, entity);

            if (success)
            {
                return OperationResult<MotoqueiroDto?>.Success(null);
            }

            return OperationResult<MotoqueiroDto?>.Failure("Falha ao atualizar o motoqueiro.");
        }

        public async Task<OperationResult<MotoqueiroDto?>> RemoverAsync(int id)
        {
            var success = await _motoqueiroRepository.DeletarAsync(id);

            if (success)
            {
                return OperationResult<MotoqueiroDto?>.Success(null);
            }

            return OperationResult<MotoqueiroDto?>.Failure("Falha ao deletar o motoqueiro.");
        }

        private async Task<MotoEntity?> AtribuirEValidarMotoAsync(int motoId, MotoqueiroEntity motoqueiroEntity)
        {
            var moto = await _motoRepository.BuscarPorIdAsync(motoId);
            if (moto == null)
                throw new Exception("Moto não encontrada");

            if (moto.Motoqueiro != null && moto.Motoqueiro.Id != motoId)
                throw new Exception("Esta moto já está associada a outro motoqueiro.");

            moto.Motoqueiro = motoqueiroEntity;
            moto.MotoqueiroId = motoqueiroEntity.Id;

            await _motoRepository.AtualizarAsync(motoId, moto);
            return moto;
        }

        public async Task<OperationResult<PageResultModel<IEnumerable<MotoqueiroDto?>>>> ObterTodos(int pageNumber = 1, int pageSize = 10)
        {

            try
            {
                var pageResult = await _motoqueiroRepository.BuscarTodos(pageNumber, pageSize);
                if (!pageResult.Items.Any()) return OperationResult<PageResultModel<IEnumerable<MotoqueiroDto?>>>.Failure("Nenhum motoqueiro encontrado.", (int)HttpStatusCode.NotFound);

                var dtos = pageResult.Items.Select(x => _mapper.Map<MotoqueiroDto>(x));

                var pageResultDto = new PageResultModel<IEnumerable<MotoqueiroDto?>>
                {
                    Items = dtos,
                    TotalItens = pageResult.TotalItens,
                    NumeroPagina = pageResult.NumeroPagina,
                    TamanhoPagina = pageResult.TamanhoPagina
                };
                
                return OperationResult<PageResultModel<IEnumerable<MotoqueiroDto?>>>.Success(pageResultDto);
            }
            catch(Exception ex)
            {
                return OperationResult<PageResultModel<IEnumerable<MotoqueiroDto?>>>.Failure($"Erro ao obter motoqueiros: {ex.Message}");
            }
           
        }
    }

}

