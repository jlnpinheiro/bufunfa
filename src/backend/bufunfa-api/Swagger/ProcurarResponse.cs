using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Collections.Generic;
using System.ComponentModel;

namespace JNogueira.Bufunfa.Api.Swagger
{
    // Classe exibida em "Model" pelo Swagger.
    public abstract class ProcurarResponse : ISaida
    {
        [Description("Indica que o request foi atendido com sucesso.")]
        public bool Sucesso { get; set; }

        [Description("Mensagens de notificações ou de exceptions encontradas durante o atendimento do request.")]
        public IEnumerable<string> Mensagens { get; set; }

        [Description("Objeto retornado com o atendimento do request.")]
        public object Retorno { get; set; }
    }
}
