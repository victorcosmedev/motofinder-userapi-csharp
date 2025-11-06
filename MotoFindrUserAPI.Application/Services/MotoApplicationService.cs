using AutoMapper;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Domain.Models.PageResultModel;
using System.Net;

namespace MotoFindrUserAPI.Application.Services;

public class MotoApplicationService : IMotoApplicationService
{
    private readonly IMotoRepository _motoRepository;
    private readonly IMotoqueiroRepository _motoqueiroRepository;
    private readonly IMapper _mapper;

    public MotoApplicationService(IMotoRepository motoRepository, IMotoqueiroRepository motoqueiroRepository, IMapper mapper)
    {
        _motoRepository = motoRepository;
        _motoqueiroRepository = motoqueiroRepository;
        _mapper = mapper;
    }

    public async Task<OperationResult<MotoDto?>> ObterPorIdAsync(int id)
    {
        try
        {
            var entity = await _motoRepository.BuscarPorIdAsync(id);
            if (entity == null)
                return OperationResult<MotoDto?>.Failure("Moto não encontrada.", 404);

            var dto = _mapper.Map<MotoDto>(entity);
            return OperationResult<MotoDto?>.Success(dto, 200);
        }
        catch (Exception ex)
        {
            return OperationResult<MotoDto?>.Failure(ex.Message, 500);
        }
    }

    public async Task<OperationResult<MotoDto?>> ObterPorPlacaAsync(string placa)
    {
        try
        {
            var entity = await _motoRepository.BuscarPorPlacaAsync(placa);
            if (entity == null)
                return OperationResult<MotoDto?>.Failure("Moto não encontrada.", 404);

            var dto = _mapper.Map<MotoDto>(entity);
            return OperationResult<MotoDto?>.Success(dto, 200);
        }
        catch (Exception ex)
        {
            return OperationResult<MotoDto?>.Failure(ex.Message, 500);
        }
    }

    public async Task<OperationResult<MotoDto?>> ObterPorChassiAsync(string chassi)
    {
        try
        {
            var entity = await _motoRepository.BuscarPorChassiAsync(chassi);
            if (entity == null)
                return OperationResult<MotoDto?>.Failure("Moto não encontrada.", 404);

            var dto = _mapper.Map<MotoDto>(entity);
            return OperationResult<MotoDto?>.Success(dto, 200);
        }
        catch (Exception ex)
        {
            return OperationResult<MotoDto?>.Failure(ex.Message, 500);
        }
    }

    public async Task<OperationResult<MotoDto?>> CriarAsync(MotoDto moto)
    {
        try
        {
            MotoqueiroEntity? motoqueiro = null;
            var entity = _mapper.Map<MotoEntity>(moto);
            if (moto.MotoqueiroId.HasValue && moto.MotoqueiroId != 0)
            {
                motoqueiro = await AtribuirEValidarMotoqueiroAsync(moto.MotoqueiroId.Value, entity);
                entity.Motoqueiro = motoqueiro;
                entity.MotoqueiroId = motoqueiro.Id;
            }
            else
            {
                entity.MotoqueiroId = null;
                entity.Motoqueiro = null;
            }


            entity = await _motoRepository.SalvarAsync(entity);
            var dto = _mapper.Map<MotoDto>(entity);

            return OperationResult<MotoDto?>.Success(dto);
        }
        catch (Exception ex)
        {
            return OperationResult<MotoDto?>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
        }
    }

    public async Task<OperationResult<MotoDto?>> AtualizarAsync(int id, MotoDto moto)
    {
        try
        {
            MotoqueiroEntity? motoqueiro = null;
            var entity = _mapper.Map<MotoEntity>(moto);
            if (moto.MotoqueiroId.HasValue && moto.MotoqueiroId != 0)
            {
                motoqueiro = await AtribuirEValidarMotoqueiroAsync(moto.MotoqueiroId.Value, entity);
            }
            else
            {
                entity.MotoqueiroId = null;
            }
            entity.Motoqueiro = motoqueiro;

            var success = await _motoRepository.AtualizarAsync(id, entity);

            if (success)
            {
                return OperationResult<MotoDto?>.Success(null);
            }

            return OperationResult<MotoDto?>.Failure("Falha ao atualizar a moto.", (int)HttpStatusCode.InternalServerError);
        }
        catch (Exception ex)
        {
            return OperationResult<MotoDto?>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
        }
        
    }

    public async Task<OperationResult<MotoDto?>> RemoverAsync(int id)
    {
        try
        {
            var success = await _motoRepository.DeletarAsync(id);
            if (success)
            {
                return OperationResult<MotoDto?>.Success(null);
            }

            return OperationResult<MotoDto?>.Failure("Falha ao deletar a moto.", (int)HttpStatusCode.InternalServerError);
        }
        catch (Exception ex)
        {
            return OperationResult<MotoDto?>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
        }
    }

    private async Task<MotoqueiroEntity> AtribuirEValidarMotoqueiroAsync(int motoqueiroId, MotoEntity motoEntity)
    {
        var motoqueiro = await _motoqueiroRepository.BuscarPorIdAsync(motoqueiroId);
        if (motoqueiro == null)
            throw new Exception("Motoqueiro não encontrado.");

        if (motoqueiro.Moto != null && motoqueiro.Moto.Id != motoEntity.Id)
            throw new Exception("Este motoqueiro já possui uma moto cadastrada.");

        motoqueiro.Moto = motoEntity;
        motoqueiro.MotoId = motoEntity.Id;

        await _motoqueiroRepository.AtualizarAsync(motoqueiroId, motoqueiro);
        return motoqueiro;
    }

    public async Task<OperationResult<PageResultModel<IEnumerable<MotoDto?>>>> ObterTodos(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var pageResult = await _motoRepository.BuscarTodos(pageNumber, pageSize);

            var dtos = pageResult.Items.Select(x => _mapper.Map<MotoDto>(x));

            var pageResultDto = new PageResultModel<IEnumerable<MotoDto?>>
            {
                Items = dtos,
                TotalItens = pageResult.TotalItens,
                NumeroPagina = pageResult.NumeroPagina,
                TamanhoPagina = pageResult.TamanhoPagina
            };

            return OperationResult<PageResultModel<IEnumerable<MotoDto?>>>.Success(pageResultDto);
        }
        catch (Exception ex)
        {
            return OperationResult<PageResultModel<IEnumerable<MotoDto?>>>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
        }
    }

    public async Task<OperationResult<IEnumerable<MotoDto>>?> ObterDadosTreinamento()
    {
        try
        {
            var entities = await _motoRepository.ObterMotosComPrecoParaTreinamento();
            var dtos = entities.Select(x => _mapper.Map<MotoDto>(x));
            return OperationResult<IEnumerable<MotoDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return OperationResult<IEnumerable<MotoDto>>.Failure(ex.Message, (int)HttpStatusCode.InternalServerError);
        }
    }
}
