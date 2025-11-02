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
        services.AddDbContext<ApplicationContext>(options => {
            options.UseOracle(configuration.GetConnectionString("Oracle"));
        });

        services.AddTransient<IMotoRepository, MotoRepository>();
        services.AddTransient<IMotoApplicationService, MotoApplicationService>();
        
        services.AddTransient<IMotoqueiroApplicationService, MotoqueiroApplicationService>();
        services.AddTransient<IMotoqueiroRepository, MotoqueiroRepository>();
        
        services.AddTransient<IEnderecoApplicationService, EnderecoApplicationService>();
        services.AddTransient<IEnderecoRepository, EnderecoRepository>();
        
        services.AddTransient<IAuthApplicationService, AuthApplicationService>();
        services.AddTransient<IUserRepository, UserRepository>();
    }

}
