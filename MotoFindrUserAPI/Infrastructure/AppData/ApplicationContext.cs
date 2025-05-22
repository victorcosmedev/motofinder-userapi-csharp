using Microsoft.EntityFrameworkCore;
using MotoFindrUserAPI.Domain.Entities;

namespace MotoFindrUserAPI.Infrastructure.AppData
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options): base(options) { }

        public DbSet<MotoEntity> Moto { get; set; }
        public DbSet<MotoqueiroEntity> Motoqueiro { get; set; }

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
        }

    }

}

