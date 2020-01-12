using JNogueira.Bufunfa.Dominio;
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
    public class ParcelaRepositorio : IParcelaRepositorio
    {
        private readonly EfDataContext _efContext;

        public ParcelaRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Parcela> ObterPorId(int idParcela, bool habilitarTracking = false)
        {
            var query = _efContext.Parcelas
                .Include(x => x.Agendamento)
                    .ThenInclude(x => x.Parcelas)
                .Include(x => x.Agendamento.Pessoa)
                .Include(x => x.Agendamento.Categoria)
                    .ThenInclude(x => x.CategoriaPai)
                    .ThenInclude(x => x.CategoriasFilha)
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idParcela);
        }

        public async Task<IEnumerable<Parcela>> ObterPorCartaoCredito(int idCartaoCredito, DateTime? dataFatura = null, bool habilitarTracking = false)
        {
            var query = (from agendamento in _efContext.Agendamentos
                         from parcela in agendamento.Parcelas
                         where agendamento.IdCartaoCredito == idCartaoCredito
                         select parcela)
                         .Include(x => x.Agendamento.Parcelas)
                         .Include(x => x.Agendamento.Pessoa)
                          .Include(x => x.Agendamento.Categoria)
                            .ThenInclude(x => x.CategoriaPai)
                            .ThenInclude(x => x.CategoriasFilha)
                          .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            if (dataFatura.HasValue)
            {
                query = query.Where(x => x.Data == dataFatura.Value.Date);
            }
            else
            {
                query = query.Where(x => !x.Lancada && !x.Descartada);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Parcela>> ObterPorFatura(int idFatura, bool habilitarTracking = false)
        {
            var query = _efContext.Parcelas.AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.Where(x => x.IdFatura == idFatura).ToListAsync();
        }

        public async Task<IEnumerable<Parcela>> ObterPorPeriodo(DateTime dataInicio, DateTime dataFim, int idUsuario, bool somenteParcelasAbertas = true)
        {
            var query = (from agendamento in _efContext.Agendamentos
                         from parcela in agendamento.Parcelas
                         where agendamento.IdUsuario == idUsuario
                         && parcela.Data >= dataInicio && parcela.Data <= dataFim
                         orderby parcela.Data ascending
                         select parcela)
                          .Include(x => x.Agendamento.Parcelas)
                          .Include(x => x.Agendamento.Conta)
                          .Include(x => x.Agendamento.CartaoCredito)
                          .Include(x => x.Agendamento.Pessoa)
                          .Include(x => x.Agendamento.Categoria)
                            .ThenInclude(x => x.CategoriaPai)
                            .ThenInclude(x => x.CategoriasFilha)
                          .AsQueryable();

            if (somenteParcelasAbertas)
                query = query.Where(x => !x.Lancada && !x.Descartada);

            return await query.ToListAsync();
        }

        public async Task Inserir(Parcela parcela)
        {
            await _efContext.AddAsync(parcela);
        }

        public async Task Inserir(IEnumerable<Parcela> parcelas)
        {
            await _efContext.AddRangeAsync(parcelas);
        }

        public void Atualizar(Parcela parcela)
        {
            _efContext.Entry(parcela).State = EntityState.Modified;
        }

        public void Deletar(Parcela parcela)
        {
            _efContext.Parcelas.Remove(parcela);
        }

        public void Deletar(IEnumerable<Parcela> parcelas)
        {
            _efContext.Parcelas.RemoveRange(parcelas);
        }

        public async Task<bool> VerificarExistenciaPorId(int idUsuario, int idParcela)
        {
            return await _efContext.Parcelas.AnyAsync(x => x.Id == idParcela);
        }
    }
}