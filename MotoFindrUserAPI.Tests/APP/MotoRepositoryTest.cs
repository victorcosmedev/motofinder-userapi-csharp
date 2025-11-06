using Microsoft.EntityFrameworkCore;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Infra.Data.AppData;
using MotoFindrUserAPI.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoFindrUserAPI.Tests.APP
{
    public class MotoRepositoryTest
    {
        [Fact]
        [Trait("Repository", "Moto")]
        public async Task SalvarAsync_DeveSalvarMotoNoBanco_ERetornarComIdGerado()
        {
            // Arrange
            await using var context = CreateContext();
            var repository = new MotoRepository(context);

            var novaMoto = new MotoEntity
            {
                Modelo = "Honda CB 500X",
                AnoDeFabricacao = 2022,
                Chassi = "9C2PC4000MR123456",
                Placa = "ABC1D23",
                MotoqueiroId = null,
                Motoqueiro = null
            };

            // Act
            var motoSalva = await repository.SalvarAsync(novaMoto);

            // Assert
            Assert.NotNull(motoSalva);
            Assert.True(motoSalva.Id > 0);
            Assert.Equal("Honda CB 500X", motoSalva.Modelo);
            Assert.Equal("9C2PC4000MR123456", motoSalva.Chassi);
            Assert.Equal("ABC1D23", motoSalva.Placa);
        }
        private ApplicationContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationContext(options);
        }
    }
}
