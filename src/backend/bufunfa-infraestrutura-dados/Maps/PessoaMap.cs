using JNogueira.Bufunfa.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Maps
{
    public class PessoaMap : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("pessoa");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("IdPessoa");
            builder.Property(x => x.IdUsuario);
            builder.Property(x => x.Nome);
        }
    }
}
