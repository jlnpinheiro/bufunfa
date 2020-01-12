using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados
{
    /// <summary>
    /// Implementação do padrão Unit Of Work
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfDataContext _context;

        public UnitOfWork(EfDataContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
