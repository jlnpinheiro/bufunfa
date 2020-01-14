using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Classe com opções de filtro para procura de lançamentos
    /// </summary>
    public class ProcurarLancamentoEntrada : ProcurarEntrada
    {
        public int? IdConta { get; set; }

        public int? IdCategoria { get; set; }

        public int? IdPessoa { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public ProcurarLancamentoEntrada(
            int idUsuario,
            string ordenarPor,
            string ordenarSentido,
            int? paginaIndex = null,
            int? paginaTamanho = null)
            : base(
                idUsuario,
                string.IsNullOrEmpty(ordenarPor) ? "Data" : ordenarPor,
                string.IsNullOrEmpty(ordenarSentido) ? "ASC" : ordenarSentido,
                paginaIndex,
                paginaTamanho)
        {
            this.Validar();
        }

        private void Validar()
        {
            if (this.DataInicio.HasValue && this.DataFim.HasValue)
                this.NotificarSeMaiorQue(this.DataInicio.Value, this.DataFim.Value, LancamentoMensagem.Lancamento_Procurar_Periodo_Invalido);

            if (this.DataInicio.HasValue && !this.DataFim.HasValue || !this.DataInicio.HasValue && this.DataFim.HasValue)
                this.NotificarSeMaiorQue(this.DataInicio.Value, this.DataFim.Value, LancamentoMensagem.Lancamento_Procurar_Periodo_Invalido);
        }
    }
}
