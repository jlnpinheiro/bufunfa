using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Infraestrutura.Dados.Maps;
using Microsoft.EntityFrameworkCore;

namespace JNogueira.Bufunfa.Infraestrutura.Dados
{
    public class EfDataContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Agendamento> Agendamentos { get; set; }
        public DbSet<Atalho> Atalhos { get; set; }
        public DbSet<CartaoCredito> CartoesCredito { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Fatura> Faturas { get; set; }
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<LancamentoAnexo> LancamentosAnexo { get; set; }
        public DbSet<LancamentoDetalhe> LancamentosDetalhe { get; set; }
        public DbSet<Parcela> Parcelas { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public EfDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeamentos para utilização do Entity Framework
            modelBuilder.ApplyConfiguration(new AgendamentoMap());
            modelBuilder.ApplyConfiguration(new AtalhoMap());
            modelBuilder.ApplyConfiguration(new CartaoCreditoMap());
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ContaMap());
            modelBuilder.ApplyConfiguration(new FaturaMap());
            modelBuilder.ApplyConfiguration(new LancamentoAnexoMap());
            modelBuilder.ApplyConfiguration(new LancamentoDetalheMap());
            modelBuilder.ApplyConfiguration(new LancamentoMap());
            modelBuilder.ApplyConfiguration(new ParcelaMap());
            modelBuilder.ApplyConfiguration(new PeriodoMap());
            modelBuilder.ApplyConfiguration(new PessoaMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
        }
    }
}
