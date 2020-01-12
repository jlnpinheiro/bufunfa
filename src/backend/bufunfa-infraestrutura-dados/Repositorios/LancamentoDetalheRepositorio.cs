using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class LancamentoDetalheRepositorio : ILancamentoDetalheRepositorio
    {
        private readonly EfDataContext _efContext;

        public LancamentoDetalheRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<LancamentoDetalhe> ObterPorId(int idDetalhe, bool habilitarTracking = false)
        {
            var query = _efContext.LancamentosDetalhe
                .Include(x => x.Lancamento)
                .Include(x => x.Categoria.CategoriaPai)
                    .ThenInclude(x => x.CategoriaPai.CategoriasFilha)
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idDetalhe);
        }

        public async Task Inserir(LancamentoDetalhe detalhe)
        {
            await _efContext.AddAsync(detalhe);
        }

        public void Deletar(LancamentoDetalhe detalhe)
        {
            _efContext.LancamentosDetalhe.Remove(detalhe);
        }
    }
}