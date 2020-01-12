using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class FaturaRepositorio : IFaturaRepositorio
    {
        private readonly EfDataContext _efContext;

        public FaturaRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Fatura> ObterPorId(int idFatura, bool habilitarTracking = false)
        {
            var query = _efContext.Faturas
                .Include(x => x.Lancamento.Conta)
                .Include(x => x.Lancamento.Categoria.CategoriaPai)
                .Include(x => x.Lancamento.Pessoa)
                .Include(x => x.CartaoCredito)
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idFatura);
        }

        public async Task<Fatura> ObterPorLancamento(int idLancamento, bool habilitarTracking = false)
        {
            var query = _efContext.Faturas
                .Include(x => x.Lancamento.Conta)
                .Include(x => x.Lancamento.Categoria.CategoriaPai)
                .Include(x => x.Lancamento.Pessoa)
                .Include(x => x.CartaoCredito)
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.IdLancamento == idLancamento);
        }

        public async Task<Fatura> ObterPorCartaoCreditoMesAno(int idCartao, int mesFatura, int anoFatura, bool habilitarTracking = false)
        {
            var query = _efContext.Faturas
                .Include(x => x.Lancamento.Conta)
                .Include(x => x.Lancamento.Categoria.CategoriaPai)
                .Include(x => x.Lancamento.Pessoa)
                .Include(x => x.CartaoCredito)
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.IdCartaoCredito == idCartao && x.MesAno == mesFatura.ToString().PadLeft(2, '0') + anoFatura.ToString());
        }

        public async Task Inserir(Fatura fatura)
        {
            await _efContext.AddAsync(fatura);
        }

        public void Deletar(Fatura fatura)
        {
            _efContext.Faturas.Remove(fatura);
        }
    }
}