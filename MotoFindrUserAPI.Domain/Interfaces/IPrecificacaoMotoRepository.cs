using MotoFindrUserAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoFindrUserAPI.Domain.Interfaces
{
    public interface IPrecificacaoMotoRepository
    {
        Task<PrecificacaoMotoEntity> SalvarOuAtualizarAsync(PrecificacaoMotoEntity entity);
        PrecificacaoMotoEntity? ObterPorMotoId(int motoId);
        IEnumerable<PrecificacaoMotoEntity> ObterDadosParaTreinamento();
    }
}
