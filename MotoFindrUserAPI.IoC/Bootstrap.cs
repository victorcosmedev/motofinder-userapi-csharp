using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Application.Services;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Infra.Data.AppData;
using MotoFindrUserAPI.Infra.Data.Repositories;

namespace MotoFindrUserAPI.IoC;

public class Bootstrap
{
    public static void AddIoC(IServiceCollection services, IConfiguration configuration)
    {
        //var connectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SID=ORCL)));User Id=rm558856;Password=fiap2025;";
        services.AddDbContext<ApplicationContext>(options => {
            options.UseOracle(configuration.GetConnectionString("Oracle"));
        });

        services.AddTransient<IMotoRepository, MotoRepository>();
        services.AddTransient<IMotoApplicationService, MotoApplicationService>();
        
        services.AddTransient<IMotoqueiroApplicationService, MotoqueiroApplicationService>();
        services.AddTransient<IMotoqueiroRepository, MotoqueiroRepository>();
        
        services.AddTransient<IEnderecoApplicationService, EnderecoApplicationService>();
        services.AddTransient<IEnderecoRepository, EnderecoRepository>();
        
        services.AddTransient<IUserApplicationService, UserApplicationService>();
        services.AddTransient<IUserRepository, UserRepository>();
    }

}
