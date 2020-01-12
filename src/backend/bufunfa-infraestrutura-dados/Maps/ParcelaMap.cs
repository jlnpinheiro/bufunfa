using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class ParcelaMap : IEntityTypeConfiguration<Parcela>
    {
        public void Configure(EntityTypeBuilder<Parcela> builder)
        {
            builder.ToTable("parcela");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("IdParcela");

            builder.Property(x => x.IdFatura);

            builder.Property(x => x.Data);

            builder.Property(x => x.Valor);

            builder.Property(x => x.Numero);

            builder.Property(x => x.Lancada);

            builder.Property(x => x.DataLancamento);

            builder.Property(x => x.Descartada);

            builder.Property(x => x.MotivoDescarte);

            builder.Property(x => x.Observacao);
        }
    }
}