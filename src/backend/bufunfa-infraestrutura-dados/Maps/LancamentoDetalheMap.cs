using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class LancamentoDetalheMap : IEntityTypeConfiguration<LancamentoDetalhe>
    {
        public void Configure(EntityTypeBuilder<LancamentoDetalhe> builder)
        {
            builder.ToTable("lancamento_detalhe");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdDetalhe");
            builder.Property(x => x.Valor);
            builder.Property(x => x.Observacao);

            builder.HasOne(x => x.Categoria)
                .WithMany()
                .HasForeignKey(x => x.IdCategoria);
        }
    }
}
