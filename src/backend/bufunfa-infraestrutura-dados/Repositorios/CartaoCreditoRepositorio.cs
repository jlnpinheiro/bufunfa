using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class CartaoCreditoRepositorio : ICartaoCreditoRepositorio
    {
        private readonly EfDataContext _efContext;

        public CartaoCreditoRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<CartaoCredito> ObterPorId(int idCartaoCredito) => await _efContext.CartoesCredito.FirstOrDefaultAsync(x => x.Id == idCartaoCredito);

        public async Task<IEnumerable<CartaoCredito>> ObterPorUsuario(int idUsuario)
        {
            return await _efContext
                   .CartoesCredito
                   .AsNoTracking()
                   .Where(x => x.IdUsuario == idUsuario)
                   .OrderBy(x => x.Nome)
                   .ToListAsync();
        }

        public async Task<bool> VerificarExistenciaPorNome(int idUsuario, string nome, int? idCartaoCredito = null)
        {
            return idCartaoCredito.HasValue
                ? await _efContext.CartoesCredito.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase) && x.Id != idCartaoCredito)
                : await _efContext.CartoesCredito.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task Inserir(CartaoCredito cartao)
        {
            await _efContext.AddAsync(cartao);
        }

        public void Atualizar(CartaoCredito cartao)
        {
            _efContext.Entry(cartao).State = EntityState.Modified;
        }

        public void Deletar(CartaoCredito cartao)
        {
            _efContext.CartoesCredito.Remove(cartao);
        }

        public async Task<bool> VerificarExistenciaPorId(int idUsuario, int idCartaoCredito)
        {
            return await _efContext.CartoesCredito.AnyAsync(x => x.IdUsuario == idUsuario && x.Id == idCartaoCredito);
        }
    }
}
