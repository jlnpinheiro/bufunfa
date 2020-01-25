using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class CartaoCreditoServico : Notificavel, ICartaoCreditoServico
    {
        private readonly ICartaoCreditoRepositorio _cartaoCreditoRepositorio;
        private readonly IContaRepositorio _contaRepositorio;
        private readonly IContaServico _contaServico;
        private readonly IFaturaRepositorio _faturaRepositorio;
        private readonly ILancamentoRepositorio _lancamentoRepositorio;
        private readonly IParcelaRepositorio _parcelaRepositorio;
        private readonly IPessoaRepositorio _pessoaRepositorio;
        private readonly IUnitOfWork _uow;

        public CartaoCreditoServico(
            ICartaoCreditoRepositorio cartaoCreditoRepositorio,
            IContaRepositorio contaRepositorio,
            IContaServico contaServico,
            IFaturaRepositorio faturaRepositorio,
            ILancamentoRepositorio lancamentoRepositorio,
            IParcelaRepositorio parcelaRepositorio,
            IPessoaRepositorio pessoaRepositorio,
            IUnitOfWork uow)
        {
            _cartaoCreditoRepositorio = cartaoCreditoRepositorio;
            _contaRepositorio         = contaRepositorio;
            _contaServico             = contaServico;
            _faturaRepositorio        = faturaRepositorio;
            _lancamentoRepositorio    = lancamentoRepositorio;
            _parcelaRepositorio       = parcelaRepositorio;
            _pessoaRepositorio        = pessoaRepositorio;
            _uow                      = uow;
        }

        public async Task<ISaida> ObterCartaoCreditoPorId(int idCartao, int idUsuario, bool calcularLimiteCreditoDisponivelAtual = true)
        {
            this
                .NotificarSeMenorOuIgualA(idCartao, 0, CartaoCreditoMensagem.Id_Cartao_Invalido)
                .NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var cartao = await _cartaoCreditoRepositorio.ObterPorId(idCartao);

            if (cartao == null)
                return new Saida(true, new[] { CartaoCreditoMensagem.Id_Cartao_Nao_Existe }, null);

            // Verifica se o cartão pertece ao usuário informado.
            this.NotificarSeDiferentes(cartao.IdUsuario, idUsuario, CartaoCreditoMensagem.Cartao_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { CartaoCreditoMensagem.Cartao_Encontrado_Com_Sucesso }, new CartaoCreditoSaida(cartao, calcularLimiteCreditoDisponivelAtual
                    ? await CalcularLimiteCreditoDisponivelAtual(cartao)
                    : (decimal?)null));
        }

        public async Task<ISaida> ObterCartoesCreditoPorUsuario(int idUsuario, bool calcularLimiteCreditoDisponivelAtual = true)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstCartoesCredito = await _cartaoCreditoRepositorio.ObterPorUsuario(idUsuario);

            if (!lstCartoesCredito.Any())
                return new Saida(true, new[] { CartaoCreditoMensagem.Nenhum_Cartao_Encontrado }, null);

            if (!calcularLimiteCreditoDisponivelAtual)
                return new Saida(true, new[] { CartaoCreditoMensagem.Cartoes_Encontrados_Com_Sucesso }, lstCartoesCredito.Select(x => new CartaoCreditoSaida(x)));

            var cartoes = new List<CartaoCreditoSaida>();

            foreach (var cartao in lstCartoesCredito)
            {
                var limiteCreditoDisponivelAtual = await CalcularLimiteCreditoDisponivelAtual(cartao);

                cartoes.Add(new CartaoCreditoSaida(cartao, limiteCreditoDisponivelAtual));
            }

            return new Saida(true, new[] { CartaoCreditoMensagem.Cartoes_Encontrados_Com_Sucesso }, cartoes);
        }

        public async Task<ISaida> CadastrarCartaoCredito(CartaoCreditoEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se o usuário já possui algum cartão com o nome informado
            this.NotificarSeVerdadeiro(await _cartaoCreditoRepositorio.VerificarExistenciaPorNome(entrada.IdUsuario, entrada.Nome), CartaoCreditoMensagem.Cartao_Com_Mesmo_Nome);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var cartao = new CartaoCredito(entrada);

            await _cartaoCreditoRepositorio.Inserir(cartao);

            await _uow.Commit();

            return new Saida(true, new[] { CartaoCreditoMensagem.Cartao_Cadastrado_Com_Sucesso }, new CartaoCreditoSaida(cartao));
        }

        public async Task<ISaida> AlterarCartaoCredito(int idCartaoCredito, CartaoCreditoEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var cartao = await _cartaoCreditoRepositorio.ObterPorId(idCartaoCredito);

            // Verifica se o cartão existe
            this.NotificarSeNulo(cartao, CartaoCreditoMensagem.Id_Cartao_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o cartão pertece ao usuário informado.
            this.NotificarSeDiferentes(cartao.IdUsuario, entrada.IdUsuario, CartaoCreditoMensagem.Cartao_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário já possui algum cartão com o nome informado
            this.NotificarSeVerdadeiro(await _cartaoCreditoRepositorio.VerificarExistenciaPorNome(entrada.IdUsuario, entrada.Nome, idCartaoCredito), CartaoCreditoMensagem.Cartao_Com_Mesmo_Nome);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            cartao.Alterar(entrada);

            _cartaoCreditoRepositorio.Atualizar(cartao);

            await _uow.Commit();

            return new Saida(true, new[] { CartaoCreditoMensagem.Cartao_Alterado_Com_Sucesso }, new CartaoCreditoSaida(cartao));
        }

        public async Task<ISaida> ExcluirCartaoCredito(int idCartao, int idUsuario)
        {
            this
                .NotificarSeMenorOuIgualA(idCartao, 0, CartaoCreditoMensagem.Id_Cartao_Invalido)
                .NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var cartao = await _cartaoCreditoRepositorio.ObterPorId(idCartao);

            // Verifica se o cartão existe
            this.NotificarSeNulo(cartao, string.Format(CartaoCreditoMensagem.Id_Cartao_Nao_Existe, idCartao));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o cartão pertece ao usuário informado.
            this.NotificarSeDiferentes(cartao.IdUsuario, idUsuario, CartaoCreditoMensagem.Cartao_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _cartaoCreditoRepositorio.Deletar(cartao);

            await _uow.Commit();

            return new Saida(true, new[] { CartaoCreditoMensagem.Cartao_Excluido_Com_Sucesso }, new CartaoCreditoSaida(cartao));
        }

        private async Task<decimal> CalcularLimiteCreditoDisponivelAtual(CartaoCredito cartaoCredito)
        {
            var parcelas = await _parcelaRepositorio.ObterPorCartaoCredito(cartaoCredito.Id);

            var totalParcelasCredito = parcelas.Where(x => x.Agendamento.Categoria.Tipo == TipoCategoria.Credito).Sum(x => x.Valor);
            var totalParcelasDebito = parcelas.Where(x => x.Agendamento.Categoria.Tipo == TipoCategoria.Debito).Sum(x => x.Valor);

            var totalParcelas = totalParcelasCredito - totalParcelasDebito;

            return cartaoCredito.ValorLimite + totalParcelas;
        }

        public async Task<ISaida> ObterFaturaPorCartaoCredito(int idCartao, int idUsuario, int mesFatura, int anoFatura)
        {
            var cartaoSaida = await this.ObterCartaoCreditoPorId(idCartao, idUsuario, false);

            if (!cartaoSaida.Sucesso)
                return cartaoSaida;

            var cartaoCredito = (CartaoCreditoSaida)cartaoSaida.Retorno;

            // Obtém todas as parcelas abertas, cuja a data seja a data do vencimento da fatura
            var parcelas = await _parcelaRepositorio.ObterPorCartaoCredito(idCartao, new DateTime(anoFatura, mesFatura, cartaoCredito.DiaVencimentoFatura));

            // Verifica se a fatura já foi lançada anteriormente
            var fatura = await _faturaRepositorio.ObterPorCartaoCreditoMesAno(idCartao, mesFatura, anoFatura);

            return (fatura != null)
                ? new Saida(true, new[] { CartaoCreditoMensagem.Fatura_Encontrada_Com_Sucesso }, new FaturaSaida(fatura, parcelas?.OrderByDescending(x => x.Id)?.Select(x => new ParcelaSaida(x))?.ToList()))
                : new Saida(true, new[] { CartaoCreditoMensagem.Fatura_Encontrada_Com_Sucesso }, new FaturaSaida(cartaoCredito, parcelas?.OrderByDescending(x => x.Id)?.Select(x => new ParcelaSaida(x))?.ToList(), mesFatura, anoFatura));
        }
        
        public async Task<ISaida> ObterFaturaPorLancamento(int idLancamento, int idUsuario)
        {
            var fatura = await _faturaRepositorio.ObterPorLancamento(idLancamento);

            this.NotificarSeNulo(fatura, CartaoCreditoMensagem.Fatura_Id_Lancamento_Nao_Encontrada);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var mesFatura = Convert.ToInt32(fatura.MesAno.Substring(0, 2));
            var anoFatura = Convert.ToInt32(fatura.MesAno.Substring(2));

            return await ObterFaturaPorCartaoCredito(fatura.IdCartaoCredito, idUsuario, mesFatura, anoFatura);
        }

        public async Task<ISaida> PagarFatura(PagarFaturaEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var cartaoSaida = await this.ObterCartaoCreditoPorId(entrada.IdCartaoCredito, entrada.IdUsuario, false);

            if (!cartaoSaida.Sucesso)
                return cartaoSaida;

            var cartaoCredito = (CartaoCreditoSaida)cartaoSaida.Retorno;

            var dataFatura = new DateTime(entrada.AnoFatura, entrada.MesFatura, cartaoCredito.DiaVencimentoFatura);

            // Obtém todas as parcelas que compôem a fatura
            var parcelas = await _parcelaRepositorio.ObterPorCartaoCredito(entrada.IdCartaoCredito, dataFatura);

            var valorFatura = parcelas?.Where(x => !x.Lancada && !x.Descartada).Sum(x => x.Valor) + (entrada.ValorAdicionalDebito.HasValue ? entrada.ValorAdicionalDebito.Value : 0) - (entrada.ValorAdicionalCredito.HasValue ? entrada.ValorAdicionalCredito.Value : 0) ?? 0;

            // Verifica se o valor do pagamento é suficiente para o pagamento da fatura
            this.NotificarSeMenorQue(entrada.ValorPagamento, valorFatura, CartaoCreditoMensagem.Fatura_Valor_Pagamento_Menor_Valor_Total_Fatura);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var contaSaida = await _contaServico.ObterContaPorId(entrada.IdContaPagamento, entrada.IdUsuario);

            if (!contaSaida.Sucesso)
                return cartaoSaida;

            var contaPagamento = (ContaSaida)contaSaida.Retorno;

            // Verifica se o saldo da conta é maior ou igual ao valor da fatura
            this.NotificarSeMenorQue(contaPagamento.ValorSaldoAtual ?? 0, entrada.ValorPagamento, string.Format(CartaoCreditoMensagem.Fatura_Saldo_Insuficiente_Pagamento_Fatura, contaPagamento.ValorSaldoAtual?.ToString("C2")));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            Pessoa pessoa = null;

            if (entrada.IdPessoaPagamento.HasValue)
            {
                pessoa = await _pessoaRepositorio.ObterPorId(entrada.IdPessoaPagamento.Value);

                this
                    .NotificarSeNulo(pessoa, PessoaMensagem.Id_Pessoa_Nao_Existe)
                    .NotificarSeDiferentes(pessoa?.IdUsuario, entrada.IdUsuario, PessoaMensagem.Pessoa_Nao_Pertence_Usuario);

                if (this.Invalido)
                    return new Saida(false, this.Mensagens, null);
            }

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lancamentoFatura = new Lancamento(new LancamentoEntrada(
                entrada.IdUsuario,
                entrada.IdContaPagamento,
                (int)TipoCategoriaEspecial.PagamentoFaturaCartao,
                entrada.DataPagamento,
                entrada.ValorPagamento,
                idPessoa: pessoa?.Id,
                observacao: $"Fatura {dataFatura.ToString("MMM").ToUpper()}/{dataFatura.ToString("yyyy")}"));

            // Insere o lançamento referente ao pagamento da fatura
            await _lancamentoRepositorio.Inserir(lancamentoFatura);

            await _uow.Commit();

            var fatura = new Fatura(
                entrada.IdCartaoCredito,
                lancamentoFatura.Id,
                entrada.MesFatura,
                entrada.AnoFatura,
                entrada.ValorAdicionalCredito,
                entrada.ObservacaoCredito,
                entrada.ValorAdicionalDebito,
                entrada.ObservacaoDebito);

            // Insere a fatura
            await _faturaRepositorio.Inserir(fatura);

            await _uow.Commit();

            foreach(var parcela in parcelas)
            {
                parcela.PagarFatura(fatura.Id, entrada.DataPagamento);
            }

            await _uow.Commit();

            fatura = await _faturaRepositorio.ObterPorId(fatura.Id);

            return new Saida(true, new[] { CartaoCreditoMensagem.Fatura_Paga_Com_Sucesso }, new FaturaSaida(fatura, parcelas.Select(x => new ParcelaSaida(x)).ToList()));
        }
    }
}
