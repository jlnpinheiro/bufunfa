using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class AgendamentoRepositorio : IAgendamentoRepositorio
    {
        private readonly EfDataContext _efContext;

        public AgendamentoRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Agendamento> ObterPorId(int idAgendamento)
        {
            return await _efContext.Agendamentos
                .Include(x => x.Conta)
                .Include(x => x.CartaoCredito)
                .Include(x => x.Categoria.CategoriaPai)
                .Include(x => x.Pessoa)
                .Include(x => x.Parcelas)
                .FirstOrDefaultAsync(x => x.Id == idAgendamento);
        }

        public async Task<ProcurarSaida> Procurar(ProcurarAgendamentoEntrada procurarEntrada)
        {
            var query = _efContext.Agendamentos
                .Include(x => x.Conta)
                .Include(x => x.CartaoCredito)
                .Include(x => x.Categoria.CategoriaPai)
                .Include(x => x.Pessoa)
                .Include(x => x.Parcelas)
                .AsNoTracking()
                .AsQueryable();

            if (procurarEntrada.IdCategoria.HasValue)
                query = query.Where(x => x.IdCategoria == procurarEntrada.IdCategoria);

            if (procurarEntrada.IdConta.HasValue)
                query = query.Where(x => x.IdConta == procurarEntrada.IdConta);

            if (procurarEntrada.IdCartaoCredito.HasValue)
                query = query.Where(x => x.IdCartaoCredito == procurarEntrada.IdCartaoCredito);

            if (procurarEntrada.IdPessoa.HasValue)
                query = query.Where(x => x.IdPessoa == procurarEntrada.IdPessoa);

            if (procurarEntrada.DataInicioParcela.HasValue && procurarEntrada.DataFimParcela.HasValue)
                query = query.Where(x => x.Parcelas.Any(y => y.Data.Date >= procurarEntrada.DataInicioParcela.Value.Date && y.Data.Date <= procurarEntrada.DataFimParcela.Value.Date));

            if (procurarEntrada.Concluido.HasValue)
            {
                query = procurarEntrada.Concluido.Value
                    ? query.AsEnumerable().Where(x => !x.Parcelas.Any(y => y.ObterStatus() == StatusParcela.Aberta)).AsQueryable()
                    : query.AsEnumerable().Where(x => x.Parcelas.Any(y => y.ObterStatus() == StatusParcela.Aberta)).AsQueryable();
            }

            if (procurarEntrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(procurarEntrada.PaginaIndex.Value, procurarEntrada.PaginaTamanho.Value);

                var lst = pagedList.ToList().OrderBy(x => x.ObterDataProximaParcelaAberta()).ThenBy(x => x.Id).Select(x => new AgendamentoSaida(x));

                return new ProcurarSaida(
                    lst,
                    "DataProximaParcelaAberta",
                    "ASC",
                    pagedList.TotalItemCount,
                    pagedList.PageCount,
                    procurarEntrada.PaginaIndex,
                    procurarEntrada.PaginaTamanho);
            }
            else
            {
                var totalRegistros = query.Count();

                return new ProcurarSaida(
                    query.ToList().OrderBy(x => x.ObterDataProximaParcelaAberta()).ThenBy(x => x.Id).Select(x => new AgendamentoSaida(x)),
                    "DataProximaParcelaAberta",
                    "ASC",
                    totalRegistros);
            }
        }

        public async Task Inserir(Agendamento agendamento)
        {
            await _efContext.AddAsync(agendamento);
        }

        public void Atualizar(Agendamento agendamento)
        {
            _efContext.Entry(agendamento).State = EntityState.Modified;
        }

        public void Deletar(Agendamento agendamento)
        {
            _efContext.Agendamentos.Remove(agendamento);
        }

        public void DeletarPorConta(int idConta)
        {
            _efContext.Agendamentos.RemoveRange(_efContext.Agendamentos.Where(x => x.IdConta == idConta));
        }
    }
}