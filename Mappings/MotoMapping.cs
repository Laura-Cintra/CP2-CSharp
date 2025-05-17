using Cp2Mottu.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cp2Mottu.Mappings
{
    public class MotoMapping : IEntityTypeConfiguration<Moto>
    {
        public void Configure(EntityTypeBuilder<Moto> builder)
        {
            builder.HasKey(m => m.Id); // Define a chave primária

            builder.Property(m => m.Placa)
                .IsRequired() // Define que a propriedade é obrigatória
                .HasMaxLength(7); // Define o tamanho máximo da string
        }
    }
}
