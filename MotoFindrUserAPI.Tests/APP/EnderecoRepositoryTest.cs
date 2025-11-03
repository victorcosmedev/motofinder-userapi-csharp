using Microsoft.EntityFrameworkCore;
using MotoFindrUserAPI.Domain.Entities;
using MotoFindrUserAPI.Infra.Data.AppData;
using MotoFindrUserAPI.Infra.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoFindrUserAPI.Tests.APP;

public class EnderecoRepositoryTest
{
    [Fact]
    [Trait("Repository", "Endereco")]
    public async Task SalvarAsync_DeveAdicionarEnderecoNoBanco_ERetornarComIdGerado()
    {
        // Arrange
        await using var context = CreateContext();

        var motoqueiroTeste = new MotoqueiroEntity
        {
            Nome = "Motoqueiro de Teste",
            Cpf = "12345678901",
            DataNascimento = new DateTime(1990, 1, 1),
            Endereco = null,
            EnderecoId = null
        };

        context.Motoqueiro.Add(motoqueiroTeste);
        await context.SaveChangesAsync();

        
        var repository = new EnderecoRepository(context);

        var novoEndereco = new EnderecoEntity
        {
            Logradouro = "Rua dos Testes",
            Complemento = "Apto 101",
            Municipio = "São Paulo",
            Uf = "SP",
            Numero = "123",
            Cep = "01001000",
            MotoqueiroId = motoqueiroTeste.Id,
            Motoqueiro = motoqueiroTeste
        };

        // Act
        var enderecoSalvo = await repository.SalvarAsync(novoEndereco);

        // Assert
        Assert.NotNull(enderecoSalvo);
        Assert.True(enderecoSalvo.Id > 0);
        Assert.Equal("Rua dos Testes", enderecoSalvo.Logradouro);
        Assert.Equal(motoqueiroTeste.Id, enderecoSalvo.MotoqueiroId);
    }
    private ApplicationContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationContext(options);
        context.Database.EnsureCreated();
        return context;
    }
}
