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
            modelBuilder.Entity<MotoqueiroEntity>()
                .HasOne(mq => mq.Moto)
                .WithOne(m => m.Motoqueiro)
                .HasForeignKey<MotoEntity>(m => m.MotoqueiroId)
                .IsRequired(false);

            base.OnModelCreating(modelBuilder);
        }

    }

}

