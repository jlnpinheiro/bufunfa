using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
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

        public async Task<LancamentoDetalhe> ObterPorId(int idDetalhe)
        {
            return await _efContext.LancamentosDetalhe
                .Include(x => x.Lancamento)
                .Include(x => x.Categoria.CategoriaPai)
                .FirstOrDefaultAsync(x => x.Id == idDetalhe);
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