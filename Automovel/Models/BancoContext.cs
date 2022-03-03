using Microsoft.EntityFrameworkCore;

namespace Automovel.Models
{
    public class BancoContext : DbContext
    {
            public DbSet<MontadoraModel> Montadoras { get; set; }
            public DbSet<AutomovelModel> Automoveis { get; set; }

        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
            {

            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MontadoraModel>().HasKey(t => t.IdMontadora);
            modelBuilder.Entity<AutomovelModel>().HasKey(t => t.IdAutomovel);

            //definimos o nome da nossa table no banco de dados
            //modelBuilder.Entity<MontadoraModel>().ToTable("Montadora");
        }

    }
}
