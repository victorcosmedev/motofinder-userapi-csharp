using Microsoft.EntityFrameworkCore;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Domain.Models.PageResultModel;
using MotoFindrUserAPI.Infra.Data.AppData;

namespace MotoFindrUserAPI.Infra.Data.Repositories
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
            motoExistente.Chassi = moto.Chassi.ToUpper();
            motoExistente.Placa = moto.Placa.ToUpper();
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
            var moto = await _context.Moto
                .FirstOrDefaultAsync(m => m.Id == id);
            return moto;
        }

        public async Task<MotoEntity?> BuscarPorPlacaAsync(string placa)
        {
            placa = placa.ToUpper();
            var moto = await _context.Moto
                .FirstOrDefaultAsync(m => m.Placa == placa);
            return moto;
        }

        public async Task<PageResultModel<IEnumerable<MotoEntity?>>> BuscarTodos(int pageNumber = 1, int pageSize = 10)
        {
            var motos = await _context.Moto
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pageResultModel = new PageResultModel<IEnumerable<MotoEntity?>>
            {
                Items = motos,
                NumeroPagina = pageNumber,
                TamanhoPagina = pageSize,
                TotalItens = await _context.Moto.CountAsync()
            };

            return pageResultModel;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var moto = _context.Moto.Find(id);
            if (moto == null)
                throw new Exception($"Moto de id {id} não existe.");

            var motoqueiro = _context.Motoqueiro.Find(moto.MotoqueiroId);
            if(motoqueiro != null)
            {
                motoqueiro.Moto = null;
                motoqueiro.MotoId = null;
            }

            _context.Moto.Remove(moto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MotoEntity> SalvarAsync(MotoEntity moto)
        {
            moto.Placa = moto.Placa.ToUpper();
            moto.Chassi = moto.Chassi.ToUpper();
            await _context.Moto.AddAsync(moto);
            await _context.SaveChangesAsync();  
            return moto;
        }
    }
}
