using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class PeriodoMap : IEntityTypeConfiguration<Periodo>
    {
        public void Configure(EntityTypeBuilder<Periodo> builder)
        {
            builder.ToTable("periodo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdPeriodo");
            builder.Property(x => x.IdUsuario);
            builder.Property(x => x.Nome);
            builder.Property(x => x.DataInicio);
            builder.Property(x => x.DataFim);
        }
    }
}
