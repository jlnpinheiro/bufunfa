using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando para padronização das saídas do domínio
    /// </summary>
    public class Saida : ISaida
    {
        public Saida()
        {

        }

        public Saida(bool sucesso, IEnumerable<string> mensagens, object retorno)
        {
            this.Sucesso = sucesso;
            this.Mensagens = mensagens;
            this.Retorno = retorno;
        }

        /// <summary>
        /// Indica se houve sucesso
        /// </summary>
        public bool Sucesso { get; set; }

        /// <summary>
        /// Mensagens retornadas
        /// </summary>
        public IEnumerable<string> Mensagens { get; set; }

        /// <summary>
        /// Objeto retornado
        /// </summary>
        public object Retorno { get; set; }
    }
}
