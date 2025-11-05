using Microsoft.EntityFrameworkCore;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Infra.Data.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoFindrUserAPI.Infra.Data.Repositories
{
    public class PrecificacaoMotoRepository : IPrecificacaoMotoRepository
    {
        private readonly ApplicationContext _context;

        public PrecificacaoMotoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<PrecificacaoMotoEntity> SalvarOuAtualizarAsync(PrecificacaoMotoEntity entity)
        {
            var precificacaoExistente = await _context.PrecificacaoMotos
                .FirstOrDefaultAsync(p => p.MotoId == entity.MotoId);

            if (precificacaoExistente != null)
            {
                precificacaoExistente.Preco = entity.Preco;
                entity = precificacaoExistente;
            }
            else
            {
                await _context.PrecificacaoMotos.AddAsync(entity);
            }

            await _context.SaveChangesAsync();
            return entity;
        }

        public PrecificacaoMotoEntity? ObterPorMotoId(int motoId)
        {
            return _context.PrecificacaoMotos
                .FirstOrDefault(p => p.MotoId == motoId);
        }

        public IEnumerable<PrecificacaoMotoEntity> ObterDadosParaTreinamento()
        {
            return _context.PrecificacaoMotos
                .Include(p => p.Moto)
                .AsNoTracking()
                .ToList();
        }
    }
}
