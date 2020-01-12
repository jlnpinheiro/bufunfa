using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class FaturaMap : IEntityTypeConfiguration<Fatura>
    {
        public void Configure(EntityTypeBuilder<Fatura> builder)
        {
            builder.ToTable("fatura");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdFatura");
            builder.Property(x => x.MesAno);
            builder.Property(x => x.ValorAdicionalCredito);
            builder.Property(x => x.ObservacaoCredito);
            builder.Property(x => x.ValorAdicionalDebito);
            builder.Property(x => x.ObservacaoDebito);

            builder.HasOne(x => x.CartaoCredito)
                .WithMany()
                .HasForeignKey(x => x.IdCartaoCredito);

            builder.HasOne(x => x.Lancamento)
                .WithMany()
                .HasForeignKey(x => x.IdLancamento);
        }
    }
}
