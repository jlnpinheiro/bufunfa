
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Bufunfa.Infraestrutura.Integracoes.AlphaVantage;
using JNogueira.NotifiqueMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class ContaServico : Notificavel, IContaServico
    {
        private readonly ApiAlphaVantageProxy _apiAlphaVantageProxy;
        private readonly IAgendamentoRepositorio _agendamentoRepositorio;
        private readonly IContaRepositorio _contaRepositorio;
        private readonly ILancamentoRepositorio _lancamentoRepositorio;
        private readonly IUnitOfWork _uow;

        public ContaServico(
            ApiAlphaVantageProxy apiAlphaVantageProxy,
            IContaRepositorio contaRepositorio,
            ILancamentoRepositorio lancamentoRepositorio,
            IAgendamentoRepositorio agendamentoRepositorio,
            IUnitOfWork uow)
        {
            _agendamentoRepositorio = agendamentoRepositorio;
            _apiAlphaVantageProxy   = apiAlphaVantageProxy;
            _contaRepositorio       = contaRepositorio;
            _lancamentoRepositorio  = lancamentoRepositorio;
            _uow                    = uow;
        }

        public async Task<ISaida> ObterContaPorId(int idConta, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idConta, 0, ContaMensagem.Id_Conta_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = await _contaRepositorio.ObterPorId(idConta);

            if (conta == null)
                return new Saida(true, new[] { ContaMensagem.Id_Conta_Nao_Existe }, null);

            // Verifica se a conta pertece ao usuário informado.
            this.NotificarSeDiferentes(conta.IdUsuario, idUsuario, ContaMensagem.Conta_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { ContaMensagem.Conta_Encontrada_Com_Sucesso }, new ContaSaida(conta, await CalcularSaldoDisponivelAtual(conta)));
        }

        public async Task<ISaida> ObterContasPorUsuario(int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstContas = await _contaRepositorio.ObterPorUsuario(idUsuario);

            return !lstContas.Any()
                ? new Saida(true, new[] { ContaMensagem.Nenhuma_Conta_Encontrada }, null)
                : new Saida(true, new[] { ContaMensagem.Contas_Encontradas_Com_Sucesso }, lstContas.Select(x => new ContaSaida(x, CalcularSaldoDisponivelAtual(x).Result)));
        }

        public async Task<ISaida> CadastrarConta(ContaEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se o usuário já possui alguma conta com o nome informado
            this.NotificarSeVerdadeiro(await _contaRepositorio.VerificarExistenciaPorNome(entrada.IdUsuario, entrada.Nome), ContaMensagem.Conta_Com_Mesmo_Nome);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = new Conta(entrada);

            await _contaRepositorio.Inserir(conta);

            await _uow.Commit();

            return new Saida(true, new[] { ContaMensagem.Conta_Cadastrada_Com_Sucesso }, new ContaSaida(conta, conta.ValorSaldoInicial));
        }

        public async Task<ISaida> AlterarConta(int idConta, ContaEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var conta = await _contaRepositorio.ObterPorId(idConta);

            // Verifica se a conta existe
            this.NotificarSeNulo(conta, string.Format(ContaMensagem.Id_Conta_Nao_Existe, idConta));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a conta pertece ao usuário informado.
            this.NotificarSeDiferentes(conta.IdUsuario, entrada.IdUsuario, ContaMensagem.Conta_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário já possui alguma conta com o nome informado
            this.NotificarSeVerdadeiro(await _contaRepositorio.VerificarExistenciaPorNome(entrada.IdUsuario, entrada.Nome, idConta), ContaMensagem.Conta_Com_Mesmo_Nome);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            conta.Alterar(entrada);

            _contaRepositorio.Atualizar(conta);

            await _uow.Commit();

            return new Saida(true, new[] { ContaMensagem.Conta_Alterada_Com_Sucesso }, new ContaSaida(conta, await CalcularSaldoDisponivelAtual(conta)));
        }

        public async Task<ISaida> ExcluirConta(int idConta, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idConta, 0, ContaMensagem.Id_Conta_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = await _contaRepositorio.ObterPorId(idConta);

            // Verifica se a conta existe
            this.NotificarSeNulo(conta, ContaMensagem.Id_Conta_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a conta pertece ao usuário informado.
            this.NotificarSeDiferentes(conta.IdUsuario, idUsuario, ContaMensagem.Conta_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _lancamentoRepositorio.DeletarPorConta(idConta);
            _agendamentoRepositorio.DeletarPorConta(idConta);

            _contaRepositorio.Deletar(conta);

            await _uow.Commit();

            return new Saida(true, new[] { ContaMensagem.Conta_Excluida_Com_Sucesso }, new ContaSaida(conta));
        }

        public async Task<ISaida> ObterAnaliseAtivo(int idConta, int idUsuario, decimal? valorCotacao = null)
        {
            var saida = await ObterContaPorId(idConta, idUsuario);

            if (!saida.Sucesso || saida.Sucesso && saida.Retorno == null)
                return saida;

            var conta = (ContaSaida)saida.Retorno;

            this.NotificarSeFalso(conta.CodigoTipo == (int)TipoConta.Acoes || conta.CodigoTipo == (int)TipoConta.FII, ContaMensagem.Analise_Carteira_Somente_Conta_Renda_Variavel);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var operacoes = await _lancamentoRepositorio.ObterPorPeriodo(conta.Id, new DateTime(2019, 1, 1), DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59));

            if (valorCotacao.HasValue && valorCotacao.Value > 0)
            {
                // O valor da cotação foi informado manualmente.
                return new Saida(true, new[] { ContaMensagem.Analise_Carteira_Obtida_Com_Sucesso }, new RendaVariavelAnaliseSaida(conta, operacoes, new RendaVariavelCotacaoSaida(valorCotacao.Value)));
            }
            else if (valorCotacao.HasValue && valorCotacao.Value == 0)
            {
                // Obtém a cotação da ação junto a API
                var apiSaida = await _apiAlphaVantageProxy.ObterCotacaoPorSiglaAtivo(conta.Nome);

                var cotacao = apiSaida != null
                    ? new RendaVariavelCotacaoSaida(apiSaida.Price, apiSaida.ChangePercent, apiSaida.LatestTradingDay)
                    : null;

                return new Saida(true, new[] { ContaMensagem.Analise_Carteira_Obtida_Com_Sucesso }, new RendaVariavelAnaliseSaida(conta, operacoes, cotacao));
            }
            else
            {
                return new Saida(true, new[] { ContaMensagem.Analise_Carteira_Obtida_Com_Sucesso }, new RendaVariavelAnaliseSaida(conta, operacoes, null));
            }
        }

        public async Task<ISaida> ObterAnaliseAtivosPorUsuario(int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstRendaVariavel = (await _contaRepositorio.ObterPorUsuario(idUsuario)).Where(x => x.Tipo == TipoConta.Acoes || x.Tipo == TipoConta.FII);

            if (lstRendaVariavel == null || !lstRendaVariavel.Any())
                return new Saida(true, new[] { ContaMensagem.Nenhuma_Conta_Encontrada }, null);

            var lstAnalise = new List<RendaVariavelAnaliseSaida>();

            foreach (var acao in lstRendaVariavel)
            {
                var analiseCarteiraSaida = await ObterAnaliseAtivo(acao.Id, idUsuario, null);

                if (analiseCarteiraSaida.Sucesso && analiseCarteiraSaida.Retorno != null)
                    lstAnalise.Add((RendaVariavelAnaliseSaida)analiseCarteiraSaida.Retorno);
            }

            return new Saida(true, new[] { ContaMensagem.Analise_Carteira_Acoes_Obtida_Com_Sucesso }, lstAnalise);
        }

        public async Task<ISaida> RealizarTransferencia(TransferenciaEntrada entrada)
        {
            // Verifica se as informações para realizar a transferência foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var contaOrigem = await _contaRepositorio.ObterPorId(entrada.IdContaOrigem);

            var contaDestino = await _contaRepositorio.ObterPorId(entrada.IdContaDestino);

            this
                .NotificarSeNulo(contaOrigem, ContaMensagem.Conta_Origem_Transferencia_Nao_Existe)
                .NotificarSeNulo(contaDestino, ContaMensagem.Conta_Destino_Transferencia_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            this
                .NotificarSeFalso(contaOrigem.IdUsuario == entrada.IdUsuario, ContaMensagem.Conta_Origem_Transferencia_Nao_Pertence_Usuario)
                .NotificarSeFalso(contaDestino.IdUsuario == entrada.IdUsuario, ContaMensagem.Conta_Destino_Transferencia_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var saldoDisponivelOrigem = await CalcularSaldoDisponivelAtual(contaOrigem);

            saldoDisponivelOrigem = saldoDisponivelOrigem ?? 0;

            // Verifica se a conta de origem possui saldo suficiente para realizar a transferência
            this.NotificarSeVerdadeiro(saldoDisponivelOrigem < entrada.Valor, string.Format(ContaMensagem.Saldo_Insuficiente_Conta_Origem_Transferencia, saldoDisponivelOrigem?.ToString("C2")));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var idTransferencia = Guid.NewGuid().ToString();

            // Lançamento de origem
            var lancamentoOrigem = new Lancamento(new LancamentoEntrada(
                entrada.IdUsuario,
                entrada.IdContaOrigem,
                (int)TipoCategoriaEspecial.TransferenciaOrigem,
                entrada.Data,
                entrada.Valor,
                observacao: !string.IsNullOrEmpty(entrada.Observacao)
                    ? $"Transferência para {contaDestino.Nome} ({entrada.Observacao})"
                    : $"Transferência para {contaDestino.Nome}"), idTransferencia);

            // Lançamento de destino
            var lancamentoDestino = new Lancamento(new LancamentoEntrada(
                entrada.IdUsuario,
                entrada.IdContaDestino,
                (int)TipoCategoriaEspecial.TransferenciaDestino,
                entrada.Data,
                entrada.Valor,
                observacao: !string.IsNullOrEmpty(entrada.Observacao)
                    ? $"Transferência de {contaOrigem.Nome} ({entrada.Observacao})"
                    : $"Transferência de {contaOrigem.Nome}"), idTransferencia);

            await _lancamentoRepositorio.Inserir(lancamentoOrigem);

            await _lancamentoRepositorio.Inserir(lancamentoDestino);

            await _uow.Commit();

            saldoDisponivelOrigem = await CalcularSaldoDisponivelAtual(contaOrigem);

            var saldoDisponivelDestino = await CalcularSaldoDisponivelAtual(contaDestino);

            var contaOrigemSaida = new ContaSaida(contaOrigem, saldoDisponivelOrigem);

            var contaDestinoSaida = new ContaSaida(contaDestino, saldoDisponivelDestino);

            return new Saida(true, new[] { ContaMensagem.Transferencia_Realizada_Com_Sucesso }, new TransferenciaSaida(contaOrigemSaida, contaDestinoSaida, entrada.Data, entrada.Valor, entrada.Observacao));
        }

        private async Task<decimal?> CalcularSaldoDisponivelAtual(Conta conta)
        {
            if (conta.Tipo == TipoConta.Acoes)
                return null;

            var dataInicio = new DateTime(2019, 1, 1);
            var dataFim = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);

            var lancamentos = await _lancamentoRepositorio.ObterPorPeriodo(conta.Id, dataInicio, dataFim);

            var totalLancamentosCredito = lancamentos.Where(x => x.Categoria.Tipo == TipoCategoria.Credito).Sum(x => x.Valor);
            var totalLancamentosDebito = lancamentos.Where(x => x.Categoria.Tipo == TipoCategoria.Debito).Sum(x => x.Valor);

            var totalLancamentos = totalLancamentosCredito - totalLancamentosDebito;

            return conta.ValorSaldoInicial.HasValue
                ? conta.ValorSaldoInicial.Value + totalLancamentos
                : totalLancamentos;
        }
    }
}
