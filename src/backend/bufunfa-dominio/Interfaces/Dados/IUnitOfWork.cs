using JNogueira.Infraestrutura.NotifiqueMe;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Contrato para utilização do padrão Unit Of Work
    /// </summary>
    public interface IUnitOfWork
    {
        Task Commit();
    }
}
