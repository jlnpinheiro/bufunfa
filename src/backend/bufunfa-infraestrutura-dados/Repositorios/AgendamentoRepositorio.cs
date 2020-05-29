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
                .Where(x => x.IdUsuario == procurarEntrada.IdUsuario)
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

            switch (procurarEntrada.OrdenarPor)
            {
                case AgendamentoOrdenarPor.DataProximaParcela:
                    query = procurarEntrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.ObterDataProximaParcelaAberta()).ThenBy(x => x.Id) : query.OrderByDescending(x => x.ObterDataProximaParcelaAberta()).ThenBy(x => x.Id);
                    break;
                case AgendamentoOrdenarPor.DataUltimaParcela:
                    query = procurarEntrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.ObterDataUltimaParcelaAberta()).ThenBy(x => x.Id) : query.OrderByDescending(x => x.ObterDataUltimaParcelaAberta()).ThenBy(x => x.Id);
                    break;
                case AgendamentoOrdenarPor.CategoriaCaminho:
                    query = procurarEntrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.Categoria.ObterCaminho()).ThenBy(x => x.Id) : query.OrderByDescending(x => x.Categoria.ObterCaminho()).ThenBy(x => x.Id);
                    break;
                case AgendamentoOrdenarPor.NomePessoa:
                    query = procurarEntrada.OrdenarSentido == "ASC" ? query.OrderBy(x => (x.Pessoa != null) ? x.Pessoa.Nome : string.Empty).ThenBy(x => x.Id) : query.OrderByDescending(x => (x.Pessoa != null) ? x.Pessoa.Nome : string.Empty).ThenBy(x => x.Id);
                    break;
                case AgendamentoOrdenarPor.NomeConta:
                    query = procurarEntrada.OrdenarSentido == "ASC" ? query.OrderBy(x => (x.Conta != null) ? x.Conta.Nome : string.Empty).ThenBy(x => x.Id) : query.OrderByDescending(x => (x.Conta != null) ? x.Conta.Nome : string.Empty).ThenBy(x => x.Id);
                    break;
                case AgendamentoOrdenarPor.NomeCartaoCredito:
                    query = procurarEntrada.OrdenarSentido == "ASC" ? query.OrderBy(x => (x.CartaoCredito != null) ? x.CartaoCredito.Nome : string.Empty).ThenBy(x => x.Id) : query.OrderByDescending(x => (x.CartaoCredito != null) ? x.CartaoCredito.Nome : string.Empty).ThenBy(x => x.Id);
                    break;
                default:
                    query = procurarEntrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                    break;
            }

            if (procurarEntrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(procurarEntrada.PaginaIndex.Value, procurarEntrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.ToList().Select(x => new AgendamentoSaida(x)),
                    procurarEntrada.OrdenarPor.ToString(),
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
                    query.ToList().Select(x => new AgendamentoSaida(x)),
                    procurarEntrada.OrdenarPor.ToString(),
                    procurarEntrada.OrdenarSentido,
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