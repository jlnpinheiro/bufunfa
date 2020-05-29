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
    public class PeriodoRepositorio : IPeriodoRepositorio
    {
        private readonly EfDataContext _efContext;

        public PeriodoRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Periodo> ObterPorId(int idPeriodo) => await _efContext.Periodos.FirstOrDefaultAsync(x => x.Id == idPeriodo);

        public async Task<Periodo> ObterPorData(DateTime data, int idUsuario)
        {
            return await _efContext
                   .Periodos
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario && x.DataInicio <= data.Date && x.DataFim >= data.Date)
                   .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Periodo>> ObterPorUsuario(int idUsuario)
        {
            return await _efContext
                   .Periodos
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario)
                   .OrderBy(x => x.DataInicio)
                   .ToListAsync();
        }

        public async Task<IEnumerable<Periodo>> ObterPorAno(int ano, int idUsuario)
        {
            return await _efContext
                   .Periodos
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario && (x.DataInicio.Year == ano || x.DataFim.Year == ano))
                   .ToListAsync();
        }

        public async Task<ProcurarSaida> Procurar(ProcurarPeriodoEntrada procurarEntrada)
        {
            var query = _efContext.Periodos
                .AsNoTracking()
                .Where(x => x.IdUsuario == procurarEntrada.IdUsuario)
                .AsQueryable();

            if (!string.IsNullOrEmpty(procurarEntrada.Nome))
                query = query.Where(x => x.Nome.IndexOf(procurarEntrada.Nome, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (procurarEntrada.Data.HasValue)
                query = query.Where(x => x.DataInicio <= procurarEntrada.Data.Value.Date && x.DataFim >= procurarEntrada.Data.Value.Date);

            switch (procurarEntrada.OrdenarPor)
            {
                case PeriodoOrdenarPor.DataFim:
                    query = procurarEntrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.DataFim) : query.OrderByDescending(x => x.DataFim);
                    break;
                case PeriodoOrdenarPor.DataInicio:
                    query = procurarEntrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.DataInicio) : query.OrderByDescending(x => x.DataInicio);
                    break;
                case PeriodoOrdenarPor.Nome:
                    query = procurarEntrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.Nome) : query.OrderByDescending(x => x.Nome);
                    break;
                default:
                    query = procurarEntrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                    break;
            }

            if (procurarEntrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(procurarEntrada.PaginaIndex.Value, procurarEntrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.ToList().Select(x => new PeriodoSaida(x)),
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
                    query.ToList().Select(x => new PeriodoSaida(x)),
                    procurarEntrada.OrdenarPor.ToString(),
                    procurarEntrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task<bool> VerificarExistenciaPorDataInicioFim(int idUsuario, DateTime dataInicio, DateTime dataFim, int? idPeriodo = null)
        {
            dataInicio = dataInicio.Date;
            dataFim = dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            return idPeriodo.HasValue
                ? await _efContext.Periodos.AnyAsync(x => x.IdUsuario == idUsuario && (x.DataInicio < dataFim && dataInicio < x.DataFim) && x.Id != idPeriodo)
                : await _efContext.Periodos.AnyAsync(x => x.IdUsuario == idUsuario && (x.DataInicio < dataFim && dataInicio < x.DataFim));
        }

        public async Task Inserir(Periodo periodo)
        {
           await _efContext.AddAsync(periodo);
        }

        public void Atualizar(Periodo periodo)
        {
            _efContext.Entry(periodo).State = EntityState.Modified;
        }

        public void Deletar(Periodo periodo)
        {
            _efContext.Periodos.Remove(periodo);
        }
    }
}