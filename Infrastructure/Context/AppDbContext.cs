using Cp2Mottu.Domain.Persistence;
using Cp2Mottu.Infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Cp2Mottu.Infrastructure.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Moto> Motos { get; set; } // Define a tabela de Motos no banco de dados - Nome da tabela é o mesmo nome da propriedade
        public DbSet<Filial> Filiais { get; set; } // Define a tabela de Filiais no banco de dados - Nome da tabela é o mesmo nome da propriedade

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MotoMapping()); // Aplica o mapeamento da entidade Moto
            modelBuilder.ApplyConfiguration(new FilialMapping()); // Aplica o mapeamento da entidade Filial
        }
    }
}
