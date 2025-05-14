using Microsoft.EntityFrameworkCore;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Infrastructure.AppData;

namespace MotoFindrUserAPI.Infrastructure.Repositories
{
    public class MotoRepository : IMotoRepository
    {
        private readonly ApplicationContext _context;
        public MotoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<bool> AtualizarAsync(int id, MotoEntity moto)
        {
            var motoExistente = await _context.Moto.FindAsync(id);
            if (motoExistente == null)
                throw new Exception($"Moto de id {id} não existe.");

            motoExistente.Modelo = moto.Modelo;
            motoExistente.AnoDeFabricacao = moto.AnoDeFabricacao;
            motoExistente.Chassi = moto.Chassi;
            motoExistente.Placa = moto.Placa;
            motoExistente.MotoqueiroId = moto.MotoqueiroId;
            motoExistente.Motoqueiro = motoExistente.Motoqueiro;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MotoEntity?> BuscarPorChassiAsync(string chassi)
        {
            chassi = chassi.ToUpper();
            var moto = await _context.Moto
                .FirstOrDefaultAsync(m =>  m.Chassi == chassi);
            return moto;
        }

        public async Task<MotoEntity?> BuscarPorIdAsync(int id)
        {
            var moto = await _context.Moto.FindAsync(id);
            return moto;
        }

        public async Task<MotoEntity?> BuscarPorPlacaAsync(string placa)
        {
            placa = placa.ToUpper();
            var moto = await _context.Moto
                .FirstOrDefaultAsync(m => m.Placa == placa);
            return moto;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var moto = _context.Moto.Find(id);
            if (moto == null)
                throw new Exception($"Moto de id {id} não existe.");
            _context.Moto.Remove(moto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MotoEntity> SalvarAsync(MotoEntity moto)
        {
            await _context.Moto.AddAsync(moto);
            await _context.SaveChangesAsync();  
            return moto;
        }
    }
}
