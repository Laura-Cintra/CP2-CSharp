using Cp2Mottu.Mappings;
using Cp2Mottu.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Cp2Mottu.Context
{
    public class MotosContext(DbContextOptions<MotosContext> options) : DbContext(options)
    {
        public DbSet<Moto> Motos { get; set; } // Define a tabela de Motos no banco de dados - Nome da tabela é o mesmo nome da propriedade
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MotoMapping()); // Aplica o mapeamento da entidade Moto
        }
    }
}
