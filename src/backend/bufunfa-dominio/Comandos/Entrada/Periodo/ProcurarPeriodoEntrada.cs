using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System;
using System.Reflection;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Classe com opções de filtro para procura de períodos
    /// </summary>
    public class ProcurarPeriodoEntrada : ProcurarEntrada
    {
        public string Nome { get; set; }

        public DateTime? Data { get; set; }

        public ProcurarPeriodoEntrada(
            int idUsuario,
            string ordenarPor,
            string ordenarSentido,
            int? paginaIndex = null,
            int? paginaTamanho = null)
            : base(
                idUsuario,
                string.IsNullOrEmpty(ordenarPor) ? "DataInicio" : ordenarPor,
                string.IsNullOrEmpty(ordenarSentido) ? "ASC" : ordenarSentido,
                paginaIndex,
                paginaTamanho)
        {
            this.Validar();
        }

        private void Validar()
        {
            this.NotificarSeNulo(typeof(Periodo).GetProperty(this.OrdenarPor, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance), string.Format(Mensagem.Paginacao_OrdernarPor_Propriedade_Nao_Existe, this.OrdenarPor));

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 50, PeriodoMensagem.Nome_Tamanho_Maximo_Excedido);

            if (this.Data.HasValue)
                this.NotificarSeMenorQue(this.Data.Value.Date, new DateTime(2015, 1, 1), string.Format(PeriodoMensagem.Periodo_Procura_Data_Invalida, new DateTime(2015, 1, 1).ToString("dd/MM/yyyy")));
        }
    }
}
