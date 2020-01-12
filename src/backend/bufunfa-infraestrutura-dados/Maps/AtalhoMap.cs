using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class AtalhoMap : IEntityTypeConfiguration<Atalho>
    {
        public void Configure(EntityTypeBuilder<Atalho> builder)
        {
            builder.ToTable("atalho");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdAtalho");
            builder.Property(x => x.IdUsuario);
            builder.Property(x => x.Titulo);
            builder.Property(x => x.Url);
        }
    }
}
