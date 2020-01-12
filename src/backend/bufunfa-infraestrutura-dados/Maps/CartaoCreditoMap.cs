using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class CartaoCreditoMap : IEntityTypeConfiguration<CartaoCredito>
    {
        public void Configure(EntityTypeBuilder<CartaoCredito> builder)
        {
            builder.ToTable("cartao_credito");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdCartaoCredito");
            builder.Property(x => x.IdUsuario);
            builder.Property(x => x.Nome);
            builder.Property(x => x.ValorLimite);
            builder.Property(x => x.DiaVencimentoFatura);
        }
    }
}
