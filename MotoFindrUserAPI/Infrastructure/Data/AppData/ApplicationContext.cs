using Microsoft.EntityFrameworkCore;
using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Infrastructure.Data.AppData
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options) { }

        public DbSet<MotoEntity> Moto { get; set; }
        public DbSet<MotoqueiroEntity> Motoqueiro { get; set; }
        public DbSet<EnderecoEntity> Endereco { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MotoEntity>()
                .HasOne(m => m.Motoqueiro)
                .WithOne(mq => mq.Moto)
                .HasForeignKey<MotoEntity>(m => m.MotoqueiroId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<MotoqueiroEntity>()
                .HasOne(mq => mq.Moto)
                .WithOne(m => m.Motoqueiro)
                .HasForeignKey<MotoqueiroEntity>(mq => mq.MotoId)
                .IsRequired(false);

            modelBuilder.Entity<MotoqueiroEntity>()
                .HasOne(mq => mq.Endereco)
                .WithOne(e => e.Motoqueiro)
                .HasForeignKey<MotoqueiroEntity>(mq => mq.EnderecoId)
                .IsRequired(false);

            modelBuilder.Entity<EnderecoEntity>()
                .HasOne(e => e.Motoqueiro)
                .WithOne(mq => mq.Endereco)
                .HasForeignKey<EnderecoEntity>(e => e.MotoqueiroId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}

