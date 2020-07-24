using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class AgendamentoServico : Notificavel, IAgendamentoServico
    {
        private readonly IAgendamentoRepositorio _agendamentoRepositorio;
        private readonly ICartaoCreditoRepositorio _cartaoCreditoRepositorio;
        private readonly ICartaoCreditoServico _cartaoCreditoServico;
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IContaRepositorio _contaRepositorio;
        private readonly ILancamentoRepositorio _lancamentoRepositorio;
        private readonly IParcelaRepositorio _parcelaRepositorio;
        private readonly IPessoaRepositorio _pessoaRepositorio;
        private readonly IUnitOfWork _uow;

        public AgendamentoServico(
            IAgendamentoRepositorio agendamentoRepositorio,
            ICartaoCreditoRepositorio cartaoCreditoRepositorio,
            ICategoriaRepositorio categoriaRepositorio,
            IContaRepositorio contaRepositorio,
            ILancamentoRepositorio lancamentoRepositorio,
            IParcelaRepositorio parcelaRepositorio,
            IPessoaRepositorio pessoaRepositorio,
            ICartaoCreditoServico cartaoCreditoServico,
            IUnitOfWork uow)
        {
            _agendamentoRepositorio   = agendamentoRepositorio;
            _cartaoCreditoRepositorio = cartaoCreditoRepositorio;
            _cartaoCreditoServico     = cartaoCreditoServico;
            _categoriaRepositorio     = categoriaRepositorio;
            _contaRepositorio         = contaRepositorio;
            _lancamentoRepositorio    = lancamentoRepositorio;
            _parcelaRepositorio       = parcelaRepositorio;
            _pessoaRepositorio        = pessoaRepositorio;
            _uow                      = uow;
        }

        public async Task<ISaida> ObterAgendamentoPorId(int idAgendamento, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idAgendamento, 0, AgendamentoMensagem.Id_Agendamento_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var agendamento = await _agendamentoRepositorio.ObterPorId(idAgendamento);

            if (agendamento == null)
                return new Saida(true, new[] { AgendamentoMensagem.Id_Agendamento_Nao_Existe }, null);

            // Verifica se o agendamento pertece ao usuário informado.
            this.NotificarSeDiferentes(agendamento.IdUsuario, idUsuario, AgendamentoMensagem.Agendamento_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { AgendamentoMensagem.Agendamento_Encontrado_Com_Sucesso }, new AgendamentoSaida(agendamento));
        }

        public async Task<ISaida> ProcurarAgendamentos(ProcurarAgendamentoEntrada entrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return entrada.Invalido
                ? new Saida(false, entrada.Mensagens, null)
                : await _agendamentoRepositorio.Procurar(entrada);
        }

        public async Task<ISaida> CadastrarAgendamento(AgendamentoEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se a categoria existe a partir do ID informado.
            this.NotificarSeFalso(await _categoriaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdCategoria), CategoriaMensagem.Id_Categoria_Nao_Existe);

            // Verifica se a conta ou cartão de crédito existem a partir do ID informado.
            if (entrada.IdConta.HasValue)
            {
                var conta = await _contaRepositorio.ObterPorId(entrada.IdConta.Value);

                this
                    .NotificarSeNulo(conta, ContaMensagem.Id_Conta_Nao_Existe)
                    .NotificarSeVerdadeiro(conta?.Tipo == TipoConta.Acoes, AgendamentoMensagem.Tipo_Conta_Invalida);
            }
            else
            {
                var cartao = (CartaoCreditoSaida)(await _cartaoCreditoServico.ObterCartaoCreditoPorId(entrada.IdCartaoCredito.Value, entrada.IdUsuario)).Retorno;

                this
                    .NotificarSeNulo(cartao, CartaoCreditoMensagem.Id_Cartao_Nao_Existe)
                    .NotificarSeVerdadeiro(cartao != null && (cartao.ValorLimiteDisponivel ?? 0) < entrada.QuantidadeParcelas * entrada.ValorParcela, $"O valor total do agendamento ({(entrada.QuantidadeParcelas * entrada.ValorParcela).ToString("C2")}) é superior ao valor do limite disponível para o cartão ({cartao.ValorLimiteDisponivel?.ToString("C2")}).");
            }

            // Verifica se a pessoa existe a partir do ID informado.
            if (entrada.IdPessoa.HasValue)
                this.NotificarSeFalso(await _pessoaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdPessoa.Value), PessoaMensagem.Id_Pessoa_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var agendamento = new Agendamento(entrada);

            await _agendamentoRepositorio.Inserir(agendamento);

            await _uow.Commit();

            return new Saida(true, new[] { AgendamentoMensagem.Agendamento_Cadastrado_Com_Sucesso }, new AgendamentoSaida(await _agendamentoRepositorio.ObterPorId(agendamento.Id)));
        }

        public async Task<ISaida> AlterarAgendamento(int idAgendamento, AgendamentoEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var agendamento = await _agendamentoRepositorio.ObterPorId(idAgendamento);

            // Verifica se o agendamento existe
            this.NotificarSeNulo(agendamento, string.Format(AgendamentoMensagem.Id_Agendamento_Nao_Existe, idAgendamento));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o agendamento pertece ao usuário informado.
            this.NotificarSeDiferentes(agendamento.IdUsuario, entrada.IdUsuario, AgendamentoMensagem.Agendamento_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a categoria existe a partir do ID informado.
            if (agendamento.IdCategoria != entrada.IdCategoria)
                this.NotificarSeFalso(await _categoriaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdCategoria), CategoriaMensagem.Id_Categoria_Nao_Existe);

            // Verifica se a conta ou cartão de crédito existem a partir do ID informado.
            if (agendamento.IdConta != entrada.IdConta && entrada.IdConta.HasValue)
                this.NotificarSeFalso(await _contaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdConta.Value), ContaMensagem.Id_Conta_Nao_Existe);
            else if (entrada.IdCartaoCredito.HasValue && agendamento.IdCartaoCredito != entrada.IdCartaoCredito)
                this.NotificarSeFalso(await _cartaoCreditoRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdCartaoCredito.Value), CartaoCreditoMensagem.Id_Cartao_Nao_Existe);

            // Verifica se a pessoa existe a partir do ID informado.
            if (agendamento.IdPessoa != entrada.IdPessoa && entrada.IdPessoa.HasValue)
                this.NotificarSeFalso(await _pessoaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdPessoa.Value), PessoaMensagem.Id_Pessoa_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Exclui todas as parcelas abertas do agendamento.
            foreach (var parcelaAberta in agendamento.Parcelas.Where(x => x.ObterStatus() == StatusParcela.Aberta))
            {
                _parcelaRepositorio.Deletar(parcelaAberta);
            }

            agendamento.Alterar(entrada);

            _agendamentoRepositorio.Atualizar(agendamento);

            await _uow.Commit();

            return new Saida(true, new[] { AgendamentoMensagem.Agendamento_Alterado_Com_Sucesso }, new AgendamentoSaida(agendamento));
        }

        public async Task<ISaida> ExcluirAgendamento(int idAgendamento, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idAgendamento, 0, AgendamentoMensagem.Id_Agendamento_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var agendamento = await _agendamentoRepositorio.ObterPorId(idAgendamento);

            // Verifica se o agendamento existe
            this.NotificarSeNulo(agendamento, AgendamentoMensagem.Id_Agendamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o agendamento pertece ao usuário informado.
            this.NotificarSeDiferentes(agendamento.IdUsuario, idUsuario, AgendamentoMensagem.Agendamento_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o agendamento possui alguma parcela lançada.
            this.NotificarSeVerdadeiro(agendamento.Parcelas.Any(x => x.Lancada), AgendamentoMensagem.Agendamento_Excluir_Possui_Parcela_Lancada);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Exclui todas as parcelas do agendamento.
            _parcelaRepositorio.Deletar(agendamento.Parcelas);

            _agendamentoRepositorio.Deletar(agendamento);

            await _uow.Commit();

            return new Saida(true, new[] { AgendamentoMensagem.Agendamento_Excluido_Com_Sucesso }, new AgendamentoSaida(agendamento));
        }

        public async Task<ISaida> ObterParcelasPorPeriodo(DateTime dataInicio, DateTime dataFim, int idUsuario, bool somenteParcelasAbertas = true)
        {
            var parcelas = await _parcelaRepositorio.ObterPorPeriodo(dataInicio, dataFim, idUsuario, somenteParcelasAbertas);

            return parcelas?.Any() == false
                ? new Saida(true, new[] { ParcelaMensagem.Nenhuma_Parcela_Encontrada }, null)
                : new Saida(true, new[] { ParcelaMensagem.Parcelas_Encontradas_Com_Sucesso }, parcelas.Select(x => new ParcelaSaida(x)));
        }

        public async Task<ISaida> CadastrarParcela(int idAgendamento, ParcelaEntrada entrada)
        {
            this.NotificarSeMenorOuIgualA(idAgendamento, 0, AgendamentoMensagem.Id_Agendamento_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var agendamento = await _agendamentoRepositorio.ObterPorId(idAgendamento);

            // Verifica se o agendamento existe
            this.NotificarSeNulo(agendamento, AgendamentoMensagem.Id_Agendamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            this
                // Verifica se o agendamento pertece ao usuário informado.
                .NotificarSeDiferentes(agendamento.IdUsuario, entrada.IdUsuario, ParcelaMensagem.Agendamento_Parcela_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var parcela = new Parcela(idAgendamento, entrada);

            await _parcelaRepositorio.Inserir(parcela);

            agendamento.AjustarNumeroParcelas();

            await _uow.Commit();

            return new Saida(true, new[] { ParcelaMensagem.Parcela_Cadastrada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> AlterarParcela(int idParcela, ParcelaEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var parcela = await _parcelaRepositorio.ObterPorId(idParcela);

            // Verifica se a parcela existe
            this.NotificarSeNulo(parcela, ParcelaMensagem.Id_Parcela_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            this
                // Verifica se a parcela pertece a um agendamento do usuário informado.
                .NotificarSeDiferentes(parcela.Agendamento.IdUsuario, entrada.IdUsuario, ParcelaMensagem.Agendamento_Parcela_Nao_Pertence_Usuario)
                // Verifica se a parcela está fechada (lançada ou descartada)
                .NotificarSeVerdadeiro(parcela.ObterStatus() == StatusParcela.Fechada, ParcelaMensagem.Parcela_Alterar_Ja_Fechada);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            parcela.Alterar(entrada);

            _parcelaRepositorio.Atualizar(parcela);

            await _uow.Commit();

            return new Saida(true, new[] { ParcelaMensagem.Parcela_Alterada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> LancarParcela(int idParcela, LancarParcelaEntrada entrada)
        {
            // Verifica se as informações para o lançamento foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var parcela = await _parcelaRepositorio.ObterPorId(idParcela);

            // Verifica se a parcela existe
            this.NotificarSeNulo(parcela, ParcelaMensagem.Id_Parcela_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela pertece a um agendamento do usuário informado.
            this.NotificarSeDiferentes(parcela.Agendamento.IdUsuario, entrada.IdUsuario, ParcelaMensagem.Agendamento_Parcela_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela já foi lançada ou descartada
            this
                .NotificarSeVerdadeiro(parcela.Lancada, ParcelaMensagem.Parcela_Lancar_Ja_Lancada)
                .NotificarSeVerdadeiro(parcela.Descartada, ParcelaMensagem.Parcela_Lancar_Ja_Descartada);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            parcela.Lancar(entrada);

            _parcelaRepositorio.Atualizar(parcela);

            // Cadastro o lançamento
            var cadastrarEntrada = new LancamentoEntrada(
                parcela.Agendamento.IdUsuario,
                parcela.Agendamento.IdConta.Value,
                parcela.Agendamento.IdCategoria,
                entrada.Data,
                entrada.Valor,
                null,
                parcela.Agendamento.IdPessoa,
                parcela.Id,
                entrada.Observacao);

            var lancamento = new Lancamento(cadastrarEntrada);

            await _lancamentoRepositorio.Inserir(lancamento);

            await _uow.Commit();

            return new Saida(true, new[] { ParcelaMensagem.Parcela_Lancada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> DescartarParcela(int idParcela, DescartarParcelaEntrada entrada)
        {
            // Verifica se as informações para descartar foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var parcela = await _parcelaRepositorio.ObterPorId(idParcela);

            // Verifica se a parcela existe
            this.NotificarSeNulo(parcela, string.Format(ParcelaMensagem.Id_Parcela_Nao_Existe, idParcela));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela já foi lançada ou descartada
            this
                .NotificarSeVerdadeiro(parcela.Lancada, ParcelaMensagem.Parcela_Lancar_Ja_Lancada)
                .NotificarSeVerdadeiro(parcela.Descartada, ParcelaMensagem.Parcela_Lancar_Ja_Descartada);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a parcela pertece a um agendamento do usuário informado.
            this.NotificarSeDiferentes(parcela.Agendamento.IdUsuario, entrada.IdUsuario, ParcelaMensagem.Agendamento_Parcela_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            parcela.Descartar(entrada);

            _parcelaRepositorio.Atualizar(parcela);

            await _uow.Commit();

            return new Saida(true, new[] { ParcelaMensagem.Parcela_Descartada_Com_Sucesso }, new ParcelaSaida(parcela));
        }

        public async Task<ISaida> ExcluirParcela(int idParcela, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idParcela, 0, ParcelaMensagem.Id_Parcela_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var parcela = await _parcelaRepositorio.ObterPorId(idParcela);

            // Verifica se a parcela existe
            this.NotificarSeNulo(parcela, ParcelaMensagem.Id_Parcela_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);
            
            this
                // Verifica se a parcela pertece a um agendamento do usuário informado.
                .NotificarSeDiferentes(parcela.Agendamento.IdUsuario, idUsuario, ParcelaMensagem.Agendamento_Parcela_Nao_Pertence_Usuario)
                // Verifica se a parcela está fechada (lançada ou descartada)
                .NotificarSeVerdadeiro(parcela.ObterStatus() == StatusParcela.Fechada, ParcelaMensagem.Parcela_Excluir_Ja_Fechada);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _parcelaRepositorio.Deletar(parcela);

            parcela.Agendamento?.RemoverParcela(parcela.Id);

            await _uow.Commit();

            return new Saida(true, new[] { ParcelaMensagem.Parcela_Excluida_Com_Sucesso }, new ParcelaSaida(parcela));
        }
    }
}