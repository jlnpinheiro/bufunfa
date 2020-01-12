using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class LancamentoAnexoMap : IEntityTypeConfiguration<LancamentoAnexo>
    {
        public void Configure(EntityTypeBuilder<LancamentoAnexo> builder)
        {
            builder.ToTable("lancamento_anexo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdAnexo");
            builder.Property(x => x.IdLancamento);
            builder.Property(x => x.IdGoogleDrive);
            builder.Property(x => x.Descricao);
            builder.Property(x => x.NomeArquivo);
        }
    }
}
