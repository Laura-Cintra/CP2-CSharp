using Cp2Mottu.Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cp2Mottu.Application.Mappings
{
    public class FilialMapping : IEntityTypeConfiguration<Filial>
    {
        public void Configure(EntityTypeBuilder<Filial> builder)
        {
            builder.HasKey(f => f.Id); // Define a chave primária

            builder.Property(f => f.Nome)
                .IsRequired() // Define que a propriedade é obrigatória
                .HasMaxLength(100); // Define o tamanho máximo da string

            builder.HasIndex(f => f.Nome)
                .IsUnique(); // Define que o nome é único

            builder.Property(f => f.Endereco)
                .IsRequired() // Define que a propriedade é obrigatória
                .HasMaxLength(200); // Define o tamanho máximo da string
        }
    }
}
