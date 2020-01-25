using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    public class AgendamentoEntrada : Notificavel
    {
        /// <summary>
        /// Id do usuário proprietário da conta
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; }

        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int? IdCartaoCredito { get; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; }

        /// <summary>
        /// Tipo do método de pagamento das parcelas
        /// </summary>
        public MetodoPagamento TipoMetodoPagamento { get; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        public string Observacao { get; }

        /// <summary>
        /// Valor da parcela
        /// </summary>
        public decimal ValorParcela { get; }

        /// <summary>
        /// Data da primeira parcela do agendamento
        /// </summary>
        public DateTime DataPrimeiraParcela { get; }

        /// <summary>
        /// Quantidade de parcelas
        /// </summary>
        public int QuantidadeParcelas { get; }

        /// <summary>
        /// Periodicidade das parcelas
        /// </summary>
        public Periodicidade PeriodicidadeParcelas { get; }
        
        public AgendamentoEntrada(
            int idUsuario,
            int idCategoria,
            int? idConta,
            int? idCartaoCredito,
            MetodoPagamento tipoMetodoPagamento,
            decimal valorParcela,
            DateTime dataPrimeiraParcela,
            int quantidadeParcelas,
            Periodicidade periodicidadeParcelas,
            int? idPessoa = null,
            string observacao = null)
        {
            this.IdUsuario             = idUsuario;
            this.IdCategoria           = idCategoria;
            this.IdConta               = idConta;
            this.IdCartaoCredito       = idCartaoCredito;
            this.TipoMetodoPagamento   = tipoMetodoPagamento;
            this.IdPessoa              = idPessoa;
            this.Observacao            = observacao;
            this.ValorParcela          = valorParcela;
            this.DataPrimeiraParcela   = dataPrimeiraParcela.Date;
            this.QuantidadeParcelas    = quantidadeParcelas;
            this.PeriodicidadeParcelas = periodicidadeParcelas;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeMenorOuIgualA(this.IdCategoria, 0, AgendamentoMensagem.Id_Categoria_Obrigatorio_Nao_Informado)
                .NotificarSeVerdadeiro(!this.IdConta.HasValue && !this.IdCartaoCredito.HasValue, AgendamentoMensagem.Id_Conta_Id_Cartao_Credito_Nao_Informados)
                .NotificarSeVerdadeiro(this.IdConta.HasValue && this.IdCartaoCredito.HasValue, AgendamentoMensagem.Id_Conta_Id_Cartao_Credito_Informados)
                .NotificarSeMenorQue(this.QuantidadeParcelas, 1, AgendamentoMensagem.Quantidade_Parcelas_Inválida);

            if (this.IdConta.HasValue)
                this.NotificarSeMenorQue(this.IdConta.Value, 1, ContaMensagem.Id_Conta_Invalido);

            if (this.IdCartaoCredito.HasValue)
                this.NotificarSeMenorQue(this.IdCartaoCredito.Value, 1, CartaoCreditoMensagem.Id_Cartao_Invalido);

            if (this.IdPessoa.HasValue)
                this.NotificarSeMenorQue(this.IdPessoa.Value, 1, PessoaMensagem.Id_Pessoa_Invalido);

            if (!string.IsNullOrEmpty(this.Observacao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Observacao, 500, AgendamentoMensagem.Observacao_Tamanho_Maximo_Excedido);
        }
    }
}
