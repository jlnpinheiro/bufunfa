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
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly EfDataContext _efContext;

        public CategoriaRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Categoria> ObterPorId(int idCategoria)
        {
            return await _efContext.Categorias
                .Include(x => x.CategoriaPai)
                .Include(x => x.CategoriasFilha)
                    .ThenInclude(y => y.CategoriasFilha)
                .FirstOrDefaultAsync(x => x.Id == idCategoria);
        }

        public async Task<ProcurarSaida> Procurar(ProcurarCategoriaEntrada procurarEntrada)
        {
            var query = _efContext.Categorias
                .Include(x => x.CategoriaPai)
                .Include(x => x.CategoriasFilha)
                    .ThenInclude(y => y.CategoriasFilha)
                .AsNoTracking()
                .Where(x => x.IdUsuario != null && x.IdUsuario == procurarEntrada.IdUsuario) // Somente as categorias sem filhas são retornadas.
                .AsQueryable();

            if (!string.IsNullOrEmpty(procurarEntrada.Nome))
                query = query.Where(x => x.Nome.IndexOf(procurarEntrada.Nome, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(procurarEntrada.Tipo))
                query = query.Where(x => x.Tipo.Equals(procurarEntrada.Tipo, StringComparison.InvariantCultureIgnoreCase));

            if (procurarEntrada.IdCategoriaPai.HasValue)
                query = query.Where(x => x.IdCategoriaPai.HasValue && x.IdCategoriaPai.Value == procurarEntrada.IdCategoriaPai.Value);

            var lst = string.IsNullOrEmpty(procurarEntrada.Caminho)
                ? (await query.ToListAsync())?
                    .OrderBy(x => x.Tipo)
                    .ThenBy(x => x.ObterCaminho()).Select(x => new CategoriaSaida(x))
                : (await query.ToListAsync())?
                    .OrderBy(x => x.Tipo)
                    .ThenBy(x => x.ObterCaminho()).Select(x => new CategoriaSaida(x))
                        .Where(x => x.Caminho.IndexOf(procurarEntrada.Caminho, StringComparison.InvariantCultureIgnoreCase) != -1);

            return new ProcurarSaida(lst);
        }

        public async Task<bool> VerificarExistenciaPorNomeTipo(int idUsuario, string nome, string tipo, int? idCategoriaPai = null, int? idCategoria = null)
        {
            return idCategoria.HasValue
                ? await _efContext.Categorias.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase) && x.Tipo.Equals(tipo, StringComparison.InvariantCultureIgnoreCase) && x.IdCategoriaPai == idCategoriaPai && x.Id != idCategoria)
                : await _efContext.Categorias.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase) && x.Tipo.Equals(tipo, StringComparison.InvariantCultureIgnoreCase) && x.IdCategoriaPai == idCategoriaPai);
        }

        public async Task<bool> VerificarExistenciaPorId(int idUsuario, int idCategoria)
        {
            return await _efContext.Categorias.AnyAsync(x => x.IdUsuario == idUsuario && x.Id == idCategoria || x.IdUsuario == null && x.Id == idCategoria);
        }

        public async Task<IEnumerable<Categoria>> ObterPorUsuario(int idUsuario)
        {
            var lst = await _efContext
                   .Categorias
                   .Include(x => x.CategoriaPai)
                   .Include(x => x.CategoriasFilha)
                        .ThenInclude(y => y.CategoriasFilha)
                   .AsNoTracking()
                   .Where(x => x.IdUsuario != null && x.IdUsuario == idUsuario)
                   .ToListAsync();

            return lst.OrderBy(x => x.Tipo).ThenBy(x => x.ObterCaminho());
        }

        public async Task Inserir(Categoria categoria)
        {
            await _efContext.AddAsync(categoria);
        }

        public void Atualizar(Categoria categoria)
        {
            _efContext.Entry(categoria).State = EntityState.Modified;
        }

        public void Deletar(Categoria categoria)
        {
            foreach (var categoriaFilha in categoria.CategoriasFilha)
            {
                Deletar(categoriaFilha);
            }

            _efContext.Categorias.Remove(categoria);
        }
    }
}