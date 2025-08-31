using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Infrastructure.AppData;

namespace MotoFindrUserAPI.Infrastructure.Repositories
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
            var enderecoExistente = await _context.Endereco.FindAsync(endereco.Id);
            
            if(enderecoExistente == null)
                throw new Exception($"Endereço com id {endereco.Id} não existe.");
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
    }
}
