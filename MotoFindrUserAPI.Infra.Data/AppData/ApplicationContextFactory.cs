using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MotoFindrUserAPI.Infra.Data.AppData
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

            string apiProjectBasePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../MotoFindrUserAPI/"));

            // 2. Ler a configuração do appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectBasePath) // Define o caminho para o projeto da API
                .AddJsonFile("appsettings.json") // Carrega o appsettings
                .Build();

            var connectionString = configuration.GetConnectionString("Oracle"); // Você usa "Oracle"

            optionsBuilder.UseOracle(connectionString); // Você usa Oracle

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
