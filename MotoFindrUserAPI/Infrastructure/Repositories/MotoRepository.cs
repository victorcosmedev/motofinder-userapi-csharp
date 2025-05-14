using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Domain.Interfaces;

namespace MotoFindrUserAPI.Infrastructure.Repositories
{
    public class MotoRepository : IMotoRepository
    {
        public Task<bool> AtualizarAsync(int id, MotoEntity moto)
        {
            throw new NotImplementedException();
        }

        public Task<MotoEntity?> BuscarPorChassiAsync(string chassi)
        {
            throw new NotImplementedException();
        }

        public Task<MotoEntity?> BuscarPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MotoEntity?> BuscarPorPlacaAsync(string placa)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletarAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MotoEntity> SalvarAsync(MotoEntity moto)
        {
            throw new NotImplementedException();
        }
    }
}
