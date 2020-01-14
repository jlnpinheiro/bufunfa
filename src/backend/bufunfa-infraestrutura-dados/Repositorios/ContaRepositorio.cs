using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class ContaRepositorio : IContaRepositorio
    {
        private readonly EfDataContext _efContext;

        public ContaRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Conta> ObterPorId(int idConta) => await _efContext.Contas.FirstOrDefaultAsync(x => x.Id == idConta);

        public async Task<IEnumerable<Conta>> ObterPorUsuario(int idUsuario)
        {
            return await _efContext.Contas
                .Where(x => x.IdUsuario == idUsuario)
                .OrderBy(x => x.Tipo)
                .ThenBy(x => x.Nome)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> VerificarExistenciaPorNome(int idUsuario, string nome, int? idConta = null)
        {
            return idConta.HasValue
                ? await _efContext.Contas.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase) && x.Id != idConta)
                : await _efContext.Contas.AnyAsync(x => x.IdUsuario == idUsuario && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<bool> VerificarExistenciaPorId(int idUsuario, int idConta)
        {
            return await _efContext.Contas.AnyAsync(x => x.IdUsuario == idUsuario && x.Id == idConta);
        }

        public async Task Inserir(Conta conta)
        {
            await _efContext.AddAsync(conta);
        }

        public void Atualizar(Conta conta)
        {
            _efContext.Entry(conta).State = EntityState.Modified;
        }

        public void Deletar(Conta conta)
        {
            _efContext.Contas.Remove(conta);
        }
    }
}
