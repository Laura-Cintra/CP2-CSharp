using Cp2Mottu.Domain.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cp2Mottu.Infrastructure.Mappings
{
    public class MotoMapping : IEntityTypeConfiguration<Moto>
    {
        public void Configure(EntityTypeBuilder<Moto> builder)
        {
            builder.HasKey(m => m.Id); // Define a chave primária

            builder.Property(m => m.Placa)
                .IsRequired() // Define que a propriedade é obrigatória
                .HasMaxLength(7); // Define o tamanho máximo da string

            builder.HasIndex(m => m.Placa)
                .IsUnique(); // Define que a placa é única

            builder.HasOne(m => m.Filial) // Define o relacionamento com a entidade Filial                
                .WithMany(f => f.Motos) // Define o relacionamento inverso, ou seja, uma filial pode ter várias motos
                .HasForeignKey(m => m.IdFilial) // Define a chave estrangeira na tabela de Motos
                .OnDelete(DeleteBehavior.Restrict); // Define o relacionamento entre Moto e Filial, ou seja, uma moto pertence a uma filial e uma filial pode ter várias motos. O comportamento de exclusão é restrito, ou seja, não permite a exclusão de uma filial se houver motos associadas a ela.
        }
    }
}
