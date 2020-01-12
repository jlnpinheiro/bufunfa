using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class LancamentoMap : IEntityTypeConfiguration<Lancamento>
    {
        public void Configure(EntityTypeBuilder<Lancamento> builder)
        {
            builder.ToTable("lancamento");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdLancamento");
            builder.Property(x => x.IdUsuario);
            builder.Property(x => x.Data);
            builder.Property(x => x.Valor);
            builder.Property(x => x.QtdRendaVariavel);
            builder.Property(x => x.IdTransferencia);
            builder.Property(x => x.Observacao);

            builder.HasOne(x => x.Conta)
                .WithMany()
                .HasForeignKey(x => x.IdConta);

            builder.HasOne(x => x.Categoria)
                .WithMany()
                .HasForeignKey(x => x.IdCategoria);

            builder.HasOne(x => x.Pessoa)
                .WithMany()
                .HasForeignKey(x => x.IdPessoa);

            builder.HasOne(x => x.Parcela)
                .WithMany()
                .HasForeignKey(x => x.IdParcela);

            builder.HasMany(x => x.Anexos)
                .WithOne(x => x.Lancamento)
                .HasForeignKey(x => x.IdLancamento)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Detalhes)
                .WithOne(x => x.Lancamento)
                .HasForeignKey(x => x.IdLancamento)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
