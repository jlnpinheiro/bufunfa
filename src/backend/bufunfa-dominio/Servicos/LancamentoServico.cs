using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class LancamentoServico : Notificavel, ILancamentoServico
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IContaRepositorio _contaRepositorio;
        private readonly IFaturaRepositorio _faturaRepositorio;
        private readonly ILancamentoAnexoRepositorio _anexoRepositorio;
        private readonly ILancamentoDetalheRepositorio _detalheRepositorio;
        private readonly ILancamentoRepositorio _lancamentoRepositorio;
        private readonly IParcelaRepositorio _parcelaRepositorio;
        private readonly IPessoaRepositorio _pessoaRepositorio;
        private readonly IUnitOfWork _uow;

        public LancamentoServico(
            ICategoriaRepositorio categoriaRepositorio,
            IContaRepositorio contaRepositorio,
            IFaturaRepositorio faturaRepositorio,
            ILancamentoAnexoRepositorio anexoRepositorio,
            ILancamentoDetalheRepositorio detalheRepositorio,
            ILancamentoRepositorio lancamentoRepositorio,
            IParcelaRepositorio parcelaRepositorio,
            IPessoaRepositorio pessoaRepositorio,
            IUnitOfWork uow)
        {
            _anexoRepositorio      = anexoRepositorio;
            _categoriaRepositorio  = categoriaRepositorio;
            _contaRepositorio      = contaRepositorio;
            _detalheRepositorio    = detalheRepositorio;
            _faturaRepositorio     = faturaRepositorio;
            _lancamentoRepositorio = lancamentoRepositorio;
            _parcelaRepositorio    = parcelaRepositorio;
            _pessoaRepositorio     = pessoaRepositorio;
            _uow                   = uow;
        }

        public async Task<ISaida> ObterLancamentoPorId(int idLancamento, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idLancamento, 0, LancamentoMensagem.Id_Lancamento_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(idLancamento);

            if (lancamento == null)
                return new Saida(true, new[] { LancamentoMensagem.Id_Lancamento_Nao_Existe }, null);

            // Verifica se o lançamento pertece ao usuário informado.
            this.NotificarSeDiferentes(lancamento.IdUsuario, idUsuario, LancamentoMensagem.Lancamento_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { LancamentoMensagem.Lancamento_Encontrado_Com_Sucesso }, new LancamentoSaida(lancamento));
        }

        public async Task<ISaida> ProcurarLancamentos(ProcurarLancamentoEntrada entrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return entrada.Invalido
                ? new Saida(false, entrada.Mensagens, null)
                : await _lancamentoRepositorio.Procurar(entrada);
        }

        public async Task<ISaida> CadastrarLancamento(LancamentoEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se a categoria existe a partir do ID informado.
            this.NotificarSeFalso(await _categoriaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdCategoria), CategoriaMensagem.Id_Categoria_Nao_Existe);

            // Verifica se a conta existe a partir do ID informado.
            this.NotificarSeFalso(await _contaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdConta), ContaMensagem.Id_Conta_Nao_Existe);

            // Verifica se a pessoa existe a partir do ID informado.
            if (entrada.IdPessoa.HasValue)
                this.NotificarSeFalso(await _pessoaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdPessoa.Value), PessoaMensagem.Id_Pessoa_Nao_Existe);

            // Verifica se a parcela existe a partir do ID informado.
            if (entrada.IdParcela.HasValue)
                this.NotificarSeFalso(await _parcelaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdParcela.Value), ParcelaMensagem.Id_Parcela_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = await _contaRepositorio.ObterPorId(entrada.IdConta);

            // Verifica se a quantidade de ações vendidas é maior que o total de ações disponíveis na carteira.
            if (conta.Tipo == TipoConta.Acoes && entrada.IdCategoria == (int)TipoCategoriaEspecial.VendaAcoes)
            {
                var operacoes = await _lancamentoRepositorio.ObterPorPeriodo(conta.Id, new DateTime(2019, 1, 1), DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59));

                var qtdAcoesCompradas = operacoes.Where(x => x.IdCategoria == (int)TipoCategoriaEspecial.CompraAcoes).Sum(x => x.QtdRendaVariavel.HasValue ? x.QtdRendaVariavel.Value : 0);
                var qtdAcoesVendidas = operacoes.Where(x => x.IdCategoria == (int)TipoCategoriaEspecial.VendaAcoes).Sum(x => x.QtdRendaVariavel.HasValue ? x.QtdRendaVariavel.Value : 0);

                var qtdAcoesDisponivel = qtdAcoesCompradas - qtdAcoesVendidas;

                this.NotificarSeVerdadeiro(entrada.QuantidadeAcoes > qtdAcoesDisponivel, LancamentoMensagem.Qtd_Acoes_Venda_Maior_Disponivel_Carteira);

                if (this.Invalido)
                    return new Saida(false, this.Mensagens, null);
            }

            var lancamento = new Lancamento(entrada);

            await _lancamentoRepositorio.Inserir(lancamento);

            await _uow.Commit();

            return new Saida(true, new[] { LancamentoMensagem.Lancamento_Cadastrado_Com_Sucesso }, new LancamentoSaida(await _lancamentoRepositorio.ObterPorId(lancamento.Id)));
        }

        public async Task<ISaida> AlterarLancamento(int idLancamento, LancamentoEntrada entrada)
        {
            this.NotificarSeMenorOuIgualA(idLancamento, 0, LancamentoMensagem.Id_Lancamento_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(idLancamento);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, LancamentoMensagem.Id_Lancamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o lançamento pertece ao usuário informado.
            this.NotificarSeDiferentes(lancamento.IdUsuario, entrada.IdUsuario, LancamentoMensagem.Lancamento_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a categoria existe a partir do ID informado.
            if (lancamento.IdCategoria != entrada.IdCategoria)
                this.NotificarSeFalso(await _categoriaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdCategoria), CategoriaMensagem.Id_Categoria_Nao_Existe);

            // Verifica se a conta existe a partir do ID informado.
            if (lancamento.IdConta != entrada.IdConta)
                this.NotificarSeFalso(await _contaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdConta), ContaMensagem.Id_Conta_Nao_Existe);

            // Verifica se a pessoa existe a partir do ID informado.
            if (lancamento.IdPessoa != entrada.IdPessoa && entrada.IdPessoa.HasValue)
                this.NotificarSeFalso(await _pessoaRepositorio.VerificarExistenciaPorId(entrada.IdUsuario, entrada.IdPessoa.Value), PessoaMensagem.Id_Pessoa_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var conta = await _contaRepositorio.ObterPorId(lancamento.IdConta);

            // Verifica se a quantidade de ações vendidas é maior que o total de ações disponíveis na carteira.
            if (conta.Tipo == TipoConta.Acoes && lancamento.IdCategoria == (int)TipoCategoriaEspecial.VendaAcoes)
            {
                var operacoes = await _lancamentoRepositorio.ObterPorPeriodo(conta.Id, new DateTime(2019, 1, 1), DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59));

                var qtdAcoesCompradas = operacoes.Where(x => x.IdCategoria == (int)TipoCategoriaEspecial.CompraAcoes).Sum(x => x.QtdRendaVariavel.HasValue ? x.QtdRendaVariavel.Value : 0);
                var qtdAcoesVendidas = operacoes.Where(x => x.IdCategoria == (int)TipoCategoriaEspecial.VendaAcoes && x.Id != idLancamento).Sum(x => x.QtdRendaVariavel.HasValue ? x.QtdRendaVariavel.Value : 0);

                var qtdAcoesDisponivel = qtdAcoesCompradas - qtdAcoesVendidas;

                this.NotificarSeVerdadeiro(entrada.QuantidadeAcoes > qtdAcoesDisponivel, LancamentoMensagem.Qtd_Acoes_Venda_Maior_Disponivel_Carteira);

                if (this.Invalido)
                    return new Saida(false, this.Mensagens, null);
            }

            lancamento.Alterar(entrada);

            _lancamentoRepositorio.Atualizar(lancamento);

            await _uow.Commit();

            return new Saida(true, new[] { LancamentoMensagem.Lancamento_Alterado_Com_Sucesso }, new LancamentoSaida(await _lancamentoRepositorio.ObterPorId(lancamento.Id)));
        }

        public async Task<ISaida> ExcluirLancamento(int idLancamento, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idLancamento, 0, LancamentoMensagem.Id_Lancamento_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(idLancamento);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, LancamentoMensagem.Id_Lancamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o lançamento pertece ao usuário informado.
            this.NotificarSeDiferentes(lancamento.IdUsuario, idUsuario, LancamentoMensagem.Lancamento_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            foreach (var anexo in lancamento.Anexos)
            {
                // Exclui os anexos do banco de dados e os arquivos do Google Drive
                await _anexoRepositorio.Deletar(anexo);
            }

            // Verifica se o lançamento está associado ao pagamento de uma fatura de cartão de crédito.
            if (lancamento.Categoria.Id == (int)TipoCategoriaEspecial.PagamentoFaturaCartao)
            {
                var fatura = await _faturaRepositorio.ObterPorLancamento(lancamento.Id);

                if (fatura != null)
                {
                    var parcelas = await _parcelaRepositorio.ObterPorFatura(fatura.Id);

                    foreach(var parcela in parcelas)
                    {
                        parcela.DesfazerLancamento();
                    }
                }
            }
            
            // Caso o lançamento esteja associado a uma parcela, a parcela deve ser reaberta.
            if (lancamento.IdParcela.HasValue)
            {
                var parcela = await _parcelaRepositorio.ObterPorId(lancamento.IdParcela.Value);

                parcela?.DesfazerLancamento();
            }

            if (string.IsNullOrEmpty(lancamento.IdTransferencia))
            {
                _lancamentoRepositorio.Deletar(lancamento);
            }
            // Caso o lançamento pertence a uma transferência, todos os lançamentos relacionados a transferência também serão excluídos.
            else
            {
                var lancamentosTransferencia = await _lancamentoRepositorio.ObterPorIdTransferencia(lancamento.IdTransferencia);

                foreach (var item in lancamentosTransferencia)
                    _lancamentoRepositorio.Deletar(item);
            }

            await _uow.Commit();

            return new Saida(true, new[] { LancamentoMensagem.Lancamento_Excluido_Com_Sucesso }, new LancamentoSaida(lancamento));
        }

        public async Task<ISaida> ObterAnexoPorId(int idAnexo, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idAnexo, 0, LancamentoAnexoMensagem.Id_Anexo_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var anexo = await _anexoRepositorio.ObterPorId(idAnexo);

            // Verifica se o anexo existe
            this.NotificarSeNulo(anexo, LancamentoAnexoMensagem.Id_Anexo_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o anexo pertece a um lançamento do usuário informado.
            this.NotificarSeDiferentes(anexo.Lancamento.IdUsuario, idUsuario, LancamentoAnexoMensagem.Anexo_Download_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { LancamentoAnexoMensagem.Anexo_Encontrado_Com_Sucesso }, new LancamentoAnexoSaida(anexo));
        }

        public async Task<ISaida> CadastrarAnexo(int idLancamento, LancamentoAnexoEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(idLancamento);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, LancamentoMensagem.Id_Lancamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Insere as informações do anexo no banco de dados e realiza o upload do arquivo para o Google Drive
            var anexo = await _anexoRepositorio.Inserir(idLancamento, lancamento.Data, entrada);

            if (_anexoRepositorio.Invalido)
                return new Saida(false, _anexoRepositorio.Mensagens, null);

            await _uow.Commit();

            return new Saida(true, new[] { LancamentoAnexoMensagem.Anexo_Cadastrado_Com_Sucesso }, new LancamentoAnexoSaida(anexo));
        }

        public async Task<ISaida> ExcluirAnexo(int idAnexo, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idAnexo, 0, LancamentoAnexoMensagem.Id_Anexo_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var anexo = await _anexoRepositorio.ObterPorId(idAnexo);

            // Verifica se o anexo existe
            this.NotificarSeNulo(anexo, LancamentoAnexoMensagem.Id_Anexo_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o anexo pertece a um lançamento ao usuário informado.
            this.NotificarSeDiferentes(anexo.Lancamento.IdUsuario, idUsuario, LancamentoAnexoMensagem.Anexo_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Exclui o anexo do banco de dados e também o arquivo do Google Drive.
            await _anexoRepositorio.Deletar(anexo);

            if (_anexoRepositorio.Invalido)
                return new Saida(false, _anexoRepositorio.Mensagens, null);

            await _uow.Commit();

            return new Saida(true, new[] { LancamentoAnexoMensagem.Anexo_Excluido_Com_Sucesso }, new LancamentoAnexoSaida(anexo));
        }

        public async Task<ISaida> CadastrarDetalhe(int idLancamento, LancamentoDetalheEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var lancamento = await _lancamentoRepositorio.ObterPorId(idLancamento);

            // Verifica se o lançamento existe
            this.NotificarSeNulo(lancamento, LancamentoMensagem.Id_Lancamento_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o valor do detalhe é maior que o valor do lançamento
            this.NotificarSeMaiorOuIgualA(entrada.Valor, lancamento.Valor, LancamentoDetalheMensagem.Valor_Detalhe_Maior_Ou_Igual_Valor_Lancamento);

            // Verifica se a categoria do detalhe é a mesma categoria informada para o lançamento.
            this.NotificarSeIguais(entrada.IdCategoria, lancamento.IdCategoria, LancamentoDetalheMensagem.Id_Categoria_Igual_Categoria_Lancamento);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a soma dos valores dos detalhes do lançamento somado ao valor do detalhe é maior que o valor do lançamento
            this.NotificarSeMaiorQue(lancamento.Detalhes.Sum(x => x.Valor) + entrada.Valor, lancamento.Valor, LancamentoDetalheMensagem.Soma_Detalhes_Mais_Valor_Detalhe_Maior_Valor_Lancamento);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var detalhe = new LancamentoDetalhe(idLancamento, entrada);

            await _detalheRepositorio.Inserir(detalhe);

            await _uow.Commit();

            return new Saida(true, new[] { LancamentoDetalheMensagem.Detalhe_Cadastrado_Com_Sucesso }, new LancamentoDetalheSaida(await _detalheRepositorio.ObterPorId(detalhe.Id)));
        }

        public async Task<ISaida> ExcluirDetalhe(int idDetalhe, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idDetalhe, 0, LancamentoDetalheMensagem.Id_Detalhe_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var detalhe = await _detalheRepositorio.ObterPorId(idDetalhe);

            // Verifica se o detalhe existe
            this.NotificarSeNulo(detalhe, LancamentoDetalheMensagem.Id_Detalhe_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o detalhe pertece a um lançamento do usuário informado.
            this.NotificarSeDiferentes(detalhe.Lancamento.IdUsuario, idUsuario, LancamentoDetalheMensagem.Detalhe_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _detalheRepositorio.Deletar(detalhe);

            await _uow.Commit();

            return new Saida(true, new[] { LancamentoDetalheMensagem.Detalhe_Excluido_Com_Sucesso }, new LancamentoDetalheSaida(detalhe));
        }
    }
}
