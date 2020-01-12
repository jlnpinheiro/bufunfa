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
    public class AtalhoRepositorio : IAtalhoRepositorio
    {
        private readonly EfDataContext _efContext;

        public AtalhoRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Atalho> ObterPorId(int idAtalho, bool habilitarTracking = false)
        {
            var query = _efContext.Atalhos.AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == idAtalho);
        }

        public async Task<IEnumerable<Atalho>> ObterPorUsuario(int idUsuario) => await _efContext.Atalhos.Where(x => x.IdUsuario == idUsuario).OrderBy(x => x.Titulo).ToListAsync();
        
        public async Task<bool> VerificarExistenciaPorTitulo(int idUsuario, string titulo, int? idAtalho = null)
        {
            return idAtalho.HasValue
                ? await _efContext.Atalhos.AnyAsync(x => x.IdUsuario == idUsuario && x.Titulo.Equals(titulo, StringComparison.InvariantCultureIgnoreCase) && x.Id != idAtalho)
                : await _efContext.Atalhos.AnyAsync(x => x.IdUsuario == idUsuario && x.Titulo.Equals(titulo, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<bool> VerificarExistenciaPorUrl(int idUsuario, string url, int? idAtalho = null)
        {
            return idAtalho.HasValue
                ? await _efContext.Atalhos.AnyAsync(x => x.IdUsuario == idUsuario && x.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase) && x.Id != idAtalho)
                : await _efContext.Atalhos.AnyAsync(x => x.IdUsuario == idUsuario && x.Url.Equals(url, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task Inserir(Atalho atalho)
        {
            await _efContext.AddAsync(atalho);
        }

        public void Atualizar(Atalho atalho)
        {
            _efContext.Entry(atalho).State = EntityState.Modified;
        }

        public void Deletar(Atalho atalho)
        {
            _efContext.Atalhos.Remove(atalho);
        }
    }
}