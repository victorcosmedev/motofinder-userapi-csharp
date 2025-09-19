using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using MotoFindrUserAPI.Application.Interfaces;
using MotoFindrUserAPI.Application.Mappers;
using MotoFindrUserAPI.Application.Services;
using MotoFindrUserAPI.Domain.Interfaces;
using MotoFindrUserAPI.Infrastructure.AppData;
using MotoFindrUserAPI.Infrastructure.Repositories;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var connectionString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle.fiap.com.br)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SID=ORCL)));User Id=rm558856;Password=fiap2025;";
builder.Services.AddDbContext<ApplicationContext>(option => {
    option.UseOracle(builder.Configuration.GetConnectionString("Oracle"));
});


builder.Services.AddTransient<IMotoApplicationService, MotoApplicationService>();
builder.Services.AddTransient<IMotoqueiroApplicationService, MotoqueiroApplicationService>();
builder.Services.AddTransient<IMotoRepository, MotoRepository>();
builder.Services.AddTransient<IMotoqueiroRepository, MotoqueiroRepository>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


builder.Services.AddSwaggerGen(conf => {

    conf.EnableAnnotations();
});

builder.Services.AddResponseCompression(options => {
    //options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options => {
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

builder.Services.AddRateLimiter(options => {
    options.AddFixedWindowLimiter(policyName: "rateLimitPolicy", opt => {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(10);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    options.AddFixedWindowLimiter(policyName: "rateLimitPolicy2", opt => {
        opt.PermitLimit = 3;
        opt.Window = TimeSpan.FromSeconds(5);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseResponseCompression();
app.UseRateLimiter();

app.MapControllers();

app.Run();
