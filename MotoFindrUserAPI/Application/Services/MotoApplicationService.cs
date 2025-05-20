using AutoMapper;
using MotoFindrUserAPI.Application.DTOs;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;

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

    public async Task<MotoDTO?> ObterPorIdAsync(int id)
    {
        var entity = await _motoRepository.BuscarPorIdAsync(id);
        return _mapper.Map<MotoDTO>(entity);
    }

    public async Task<MotoDTO?> ObterPorPlacaAsync(string placa)
    {
        var entity = await _motoRepository.BuscarPorPlacaAsync(placa);
        return _mapper.Map<MotoDTO>(entity);
    }

    public async Task<MotoDTO?> ObterPorChassiAsync(string chassi)
    {
        var entity = await _motoRepository.BuscarPorChassiAsync(chassi);
        return _mapper.Map<MotoDTO>(entity);
    }

    public async Task<MotoDTO> CriarAsync(MotoDTO moto)
    {
        MotoqueiroEntity? motoqueiro = null;
        var entity = _mapper.Map<MotoEntity>(moto);
        if (moto.MotoqueiroId.HasValue && moto.MotoqueiroId != 0)
        {
            motoqueiro = await AtribuirEValidarMotoqueiroAsync(moto.MotoqueiroId.Value, entity);
        }

        entity.Motoqueiro = motoqueiro;
        entity = await _motoRepository.SalvarAsync(entity);

        return _mapper.Map<MotoDTO>(entity);
    }

    public async Task<bool> AtualizarAsync(int id, MotoDTO moto)
    {
        MotoqueiroEntity? motoqueiro = null;
        var entity = _mapper.Map<MotoEntity>(moto);
        if (moto.MotoqueiroId.HasValue && moto.MotoqueiroId != 0)
        {
            motoqueiro = await AtribuirEValidarMotoqueiroAsync(moto.MotoqueiroId.Value, entity);
        }
        entity.Motoqueiro = motoqueiro;

        return await _motoRepository.AtualizarAsync(id, entity);
    }

    public async Task<bool> RemoverAsync(int id)
    {
        return await _motoRepository.DeletarAsync(id);
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

    public async Task<IEnumerable<MotoDTO?>> ObterTodos()
    {
        var entities = await _motoRepository.BuscarTodos();
        return entities.Select(x => _mapper.Map<MotoDTO>(x));
    }
}
