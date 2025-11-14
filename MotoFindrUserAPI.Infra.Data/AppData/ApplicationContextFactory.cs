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

            
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectBasePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Oracle");

            optionsBuilder.UseOracle(connectionString);

            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
