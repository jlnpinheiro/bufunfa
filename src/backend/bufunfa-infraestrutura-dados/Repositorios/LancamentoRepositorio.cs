using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class LancamentoRepositorio : ILancamentoRepositorio
    {
        private readonly EfDataContext _efContext;

        public LancamentoRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Lancamento> ObterPorId(int idLancamento)
        {
            return await _efContext.Lancamentos
                .Include(x => x.Conta)
                .Include(x => x.Categoria.CategoriaPai)
                .Include(x => x.Pessoa)
                .Include(x => x.Parcela)
                .Include(x => x.Anexos)
                .Include(x => x.Detalhes)
                    .ThenInclude(x => x.Categoria.CategoriaPai)
                .FirstOrDefaultAsync(x => x.Id == idLancamento);
        }

        public async Task<IEnumerable<Lancamento>> ObterPorIdTransferencia(string idTransferencia) => await _efContext.Lancamentos.Where(x => x.IdTransferencia == idTransferencia).ToListAsync();

        public async Task<ProcurarSaida> Procurar(ProcurarLancamentoEntrada procurarEntrada)
        {
            var query = _efContext.Lancamentos
                .Include(x => x.Conta)
                .Include(x => x.Categoria.CategoriaPai)
                .Include(x => x.Pessoa)
                .Include(x => x.Parcela)
                .Include(x => x.Anexos)
                .Include(x => x.Detalhes)
                    .ThenInclude(x => x.Categoria.CategoriaPai)
                .AsNoTracking()
                .AsQueryable();

            if (procurarEntrada.IdConta.HasValue)
                query = query.Where(x => x.IdConta == procurarEntrada.IdConta.Value);

            if (procurarEntrada.IdCategoria.HasValue)
                query = query.Where(x => x.IdCategoria == procurarEntrada.IdCategoria.Value);

            if (procurarEntrada.IdPessoa.HasValue)
                query = query.Where(x => x.IdPessoa == procurarEntrada.IdPessoa.Value);

            if (procurarEntrada.DataInicio.HasValue && procurarEntrada.DataFim.HasValue)
                query = query.Where(x => x.Data.Date >= procurarEntrada.DataInicio.Value.Date && x.Data.Date <= procurarEntrada.DataFim.Value.Date);

            switch(procurarEntrada.OrdenarPor)
            {
                case "pessoa":
                    query = string.Equals(procurarEntrada.OrdenarSentido, "ASC", StringComparison.OrdinalIgnoreCase) ? query.OrderBy(x => x.Pessoa.Nome) : query.OrderByDescending(x => x.Pessoa.Nome);
                    break;
                default:
                    query = query.OrderByProperty(procurarEntrada.OrdenarPor, procurarEntrada.OrdenarSentido);
                    break;
            }

            if (procurarEntrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(procurarEntrada.PaginaIndex.Value, procurarEntrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.ToList().Select(x => new LancamentoSaida(x)),
                    procurarEntrada.OrdenarPor,
                    procurarEntrada.OrdenarSentido,
                    pagedList.TotalItemCount,
                    pagedList.PageCount,
                    procurarEntrada.PaginaIndex,
                    procurarEntrada.PaginaTamanho);
            }
            else
            {
                var totalRegistros = query.Count();

                return new ProcurarSaida(
                    query.ToList().Select(x => new LancamentoSaida(x)),
                    procurarEntrada.OrdenarPor,
                    procurarEntrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task<IEnumerable<Lancamento>> ObterPorPeriodo(int idConta, DateTime dataInicio, DateTime dataFim)
        {
            return await _efContext
                   .Lancamentos
                   .Include(x => x.Categoria.CategoriaPai)
                   .Include(x => x.Pessoa)
                   .Include(x => x.Parcela)
                   .AsNoTracking()
                   .Where(x => x.IdConta == idConta && x.Data >= dataInicio && x.Data <= dataFim)
                   .OrderBy(x => x.Data)
                   .ToListAsync();
        }

        public async Task Inserir(Lancamento lancamento)
        {
            await _efContext.AddAsync(lancamento);
        }

        public void Atualizar(Lancamento lancamento)
        {
            _efContext.Entry(lancamento).State = EntityState.Modified;
        }

        public void Deletar(Lancamento lancamento)
        {
            _efContext.Lancamentos.Remove(lancamento);
        }

        public void DeletarPorConta(int idConta)
        {
            _efContext.Lancamentos.RemoveRange(_efContext.Lancamentos.Where(x => x.IdConta == idConta));
        }
    }
}