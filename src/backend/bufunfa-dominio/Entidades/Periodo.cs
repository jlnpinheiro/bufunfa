using JNogueira.Bufunfa.Dominio.Comandos;
using System;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma período
    /// </summary>
    public class Periodo
    {
        /// <summary>
        /// ID do período
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Nome da período
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Data início do período
        /// </summary>
        public DateTime DataInicio { get; private set; }

        /// <summary>
        /// Data fim do período
        /// </summary>
        public DateTime DataFim { get; private set; }


        private Periodo()
        {
        }

        public Periodo(PeriodoEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.IdUsuario  = entrada.IdUsuario;
            this.Nome       = entrada.Nome;
            this.DataInicio = entrada.DataInicio;
            this.DataFim    = entrada.DataFim;
        }

        public void Alterar(PeriodoEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Nome       = entrada.Nome;
            this.DataInicio = entrada.DataInicio;
            this.DataFim    = entrada.DataFim;
        }

        public override string ToString()
        {
            return $"{this.Nome} - {this.DataInicio.ToString("dd/MM/yyyy")} até {this.DataFim.ToString("dd/MM/yyyy")}";
        }
    }
}