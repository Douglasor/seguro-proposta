using Microsoft.EntityFrameworkCore;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;

namespace PropostaService.Infrastructure.Data
{
    public class PropostaDbContext : DbContext
    {
        public PropostaDbContext(DbContextOptions<PropostaDbContext> options) : base(options)
        {
        }

        public DbSet<Proposta> Propostas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Proposta>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id)
                    .IsRequired();

                entity.Property(e => e.ClienteId)
                    .IsRequired();

                entity.Property(e => e.DataCriacao)
                    .IsRequired();

                entity.Property(e => e.ValorProposta)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasConversion<int>()
                    .IsRequired();

                entity.ToTable("Propostas");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

