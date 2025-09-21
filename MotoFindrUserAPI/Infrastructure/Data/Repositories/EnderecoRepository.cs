using Microsoft.EntityFrameworkCore;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Infrastructure.Data.AppData;
using MotoFindrUserAPI.Models.PageResultModel;

namespace MotoFindrUserAPI.Infrastructure.Data.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly ApplicationContext _context;
        public EnderecoRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<bool> AtualizarAsync(int id, EnderecoEntity endereco)
        {
            var enderecoExistente = await _context.Endereco.FindAsync(id);
            
            if(enderecoExistente == null)
                throw new Exception($"Endereço com id {id} não existe.");
            enderecoExistente.Logradouro = endereco.Logradouro;
            enderecoExistente.Complemento = endereco.Complemento;
            enderecoExistente.Municipio = endereco.Municipio;
            enderecoExistente.Uf = endereco.Uf;
            enderecoExistente.Cep = endereco.Cep;
            enderecoExistente.Numero = endereco.Numero;
            if(enderecoExistente.MotoqueiroId != endereco.MotoqueiroId)
                throw new Exception("Não é permitido alterar o motoqueiro associado a este endereço.");
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EnderecoEntity?> BuscarPorIdAsync(int id)
        {
            return await _context.Endereco.FindAsync(id);
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var endereco = await _context.Endereco.FindAsync(id);
            if(endereco == null)
                throw new Exception($"Endereço de id {id} não existe");

            var motoqueiro = await _context.Motoqueiro.FindAsync(endereco.MotoqueiroId);
            if(motoqueiro != null)
            {
                motoqueiro.Endereco = null;
                motoqueiro.EnderecoId = null;
            }

            _context.Endereco.Remove(endereco);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EnderecoEntity> SalvarAsync(EnderecoEntity endereco)
        {
            await _context.Endereco.AddAsync(endereco);
            await _context.SaveChangesAsync();
            return endereco;
        }

        public async Task<PageResultModel<IEnumerable<EnderecoEntity?>>> BuscarTodos(int pageNumber = 1, int pageSize = 10)
        {
            var endereco = await _context.Endereco
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pageResultModel = new PageResultModel<IEnumerable<EnderecoEntity?>>
            {
                Items = endereco,
                NumeroPagina = pageNumber,
                TamanhoPagina = pageSize,
                TotalItens = await _context.Endereco.CountAsync()
            };

            return pageResultModel;
        }
    }
}
