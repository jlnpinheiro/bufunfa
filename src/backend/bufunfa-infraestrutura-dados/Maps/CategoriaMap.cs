using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("categoria");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdCategoria");
            builder.Property(x => x.IdCategoriaPai);

            builder.HasOne(x => x.CategoriaPai)
                .WithMany(y => y.CategoriasFilha)
                .HasForeignKey(x => x.IdCategoriaPai);

            builder.Property(x => x.IdUsuario);
            builder.Property(x => x.Nome);
            builder.Property(x => x.Tipo);
        }
    }
}
