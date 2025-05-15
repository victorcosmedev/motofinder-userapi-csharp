using Microsoft.EntityFrameworkCore;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Infrastructure.AppData;

namespace MotoFindrUserAPI.Infrastructure.Repositories
{
    public class MotoqueiroRepository : IMotoqueiroRepository
    {
        private readonly ApplicationContext _context;

        public MotoqueiroRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<MotoqueiroEntity> SalvarAsync(MotoqueiroEntity motoqueiro)
        {
            await _context.Motoqueiro.AddAsync(motoqueiro);
            await _context.SaveChangesAsync();
            return motoqueiro;
        }
        public async Task<bool> AtualizarAsync(int id, MotoqueiroEntity motoqueiro)
        {
            var motoqueiroExistente = await _context.Motoqueiro.FindAsync(motoqueiro.Id);
            if (motoqueiroExistente == null)
                throw new Exception($"Motoqueiro com id {motoqueiro.Id} não existe.");
            motoqueiroExistente.Nome = motoqueiro.Nome;
            motoqueiroExistente.Cpf = motoqueiro.Cpf;
            motoqueiroExistente.Endereco = motoqueiro.Endereco;
            motoqueiroExistente.DataNascimento = motoqueiro.DataNascimento;
            motoqueiroExistente.MotoId = motoqueiro.MotoId;
            motoqueiroExistente.Moto = motoqueiro.Moto;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MotoqueiroEntity?> BuscarPorCpfAsync(string cpf)
        {
            return await _context.Motoqueiro
                .FirstOrDefaultAsync(m => m.Cpf == cpf);
        }

        public async Task<MotoqueiroEntity?> BuscarPorIdAsync(int id)
        {
            return await _context.Motoqueiro.FindAsync(id);
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var motoqueiro = await _context.Motoqueiro.FindAsync(id);
            if (motoqueiro == null)
                throw new Exception($"Motoqueiro de id {id} não existe");

            var moto = await _context.Moto.FindAsync(motoqueiro.MotoId);
            if(moto != null)
            {
                moto.Motoqueiro = null;
                moto.MotoqueiroId = null;
            }

             _context.Motoqueiro.Remove(motoqueiro!);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MotoqueiroEntity?>> BuscarTodos()
        {
            var motoqueiros = await _context.Motoqueiro.ToListAsync();
            return motoqueiros;
        }
    }
}
