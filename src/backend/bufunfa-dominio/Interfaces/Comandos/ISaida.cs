using System.Collections.Generic;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Comandos
{
    public interface ISaida
    {
        bool Sucesso { get; set; }

        IEnumerable<string> Mensagens { get; set; }

        object Retorno { get; set; }
    }
}
