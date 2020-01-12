using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class PeriodoServico : Notificavel, IPeriodoServico
    {
        private readonly IPeriodoRepositorio _periodoRepositorio;
        private readonly IUnitOfWork _uow;

        public PeriodoServico(
            IPeriodoRepositorio periodoRepositorio,
            IUnitOfWork uow)
        {
            _periodoRepositorio = periodoRepositorio;
            _uow                = uow;
        }

        public async Task<ISaida> ObterPeriodoPorId(int idPeriodo, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idPeriodo, 0, PeriodoMensagem.Id_Periodo_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var periodo = await _periodoRepositorio.ObterPorId(idPeriodo);

            if (periodo == null)
                return new Saida(true, new[] { string.Format(PeriodoMensagem.Id_Periodo_Nao_Existe, idPeriodo) } , null);

            // Verifica se o período pertece ao usuário informado.
            this.NotificarSeDiferentes(periodo.IdUsuario, idUsuario, PeriodoMensagem.Periodo_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { PeriodoMensagem.Periodo_Encontrado_Com_Sucesso }, new PeriodoSaida(periodo));
        }

        public async Task<ISaida> ObterPeriodoPorData(DateTime data, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var periodo = await _periodoRepositorio.ObterPorData(data, idUsuario);

            if (periodo == null)
                return new Saida(true, new[] { PeriodoMensagem.Data_Periodo_Nao_Existe }, null);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { PeriodoMensagem.Periodo_Encontrado_Com_Sucesso }, new PeriodoSaida(periodo));
        }

        public async Task<ISaida> ObterPeriodosPorUsuario(int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstPeriodos = await _periodoRepositorio.ObterPorUsuario(idUsuario);

            return lstPeriodos.Any()
                ? new Saida(true, new[] { PeriodoMensagem.Periodos_Encontrados_Com_Sucesso }, lstPeriodos.Select(x => new PeriodoSaida(x)))
                : new Saida(true, new[] { PeriodoMensagem.Nenhum_periodo_encontrado }, null);
        }

        public async Task<ISaida> ProcurarPeriodos(ProcurarPeriodoEntrada entrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return entrada.Invalido
                ? new Saida(false, entrada.Mensagens, null)
                : await _periodoRepositorio.Procurar(entrada);
        }

        public async Task<ISaida> CadastrarPeriodo(PeriodoEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se já existe um período que abrange as datas informadas
            this.NotificarSeVerdadeiro(
                await _periodoRepositorio.VerificarExistenciaPorDataInicioFim(entrada.IdUsuario, entrada.DataInicio, entrada.DataFim),
                PeriodoMensagem.Datas_Abrangidas_Outro_Periodo);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var periodo = new Periodo(entrada);

            await _periodoRepositorio.Inserir(periodo);

            await _uow.Commit();

            return new Saida(true, new[] { PeriodoMensagem.Periodo_Cadastrado_Com_Sucesso }, new PeriodoSaida(periodo));
        }

        public async Task<ISaida> AlterarPeriodo(int idPeriodo, PeriodoEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var periodo = await _periodoRepositorio.ObterPorId(idPeriodo, true);

            // Verifica se o período existe
            this.NotificarSeNulo(periodo, PeriodoMensagem.Id_Periodo_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o período pertece ao usuário informado.
            this.NotificarSeDiferentes(periodo.IdUsuario, entrada.IdUsuario, PeriodoMensagem.Periodo_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe um período que abrange as datas informadas
            this.NotificarSeVerdadeiro(
                await _periodoRepositorio.VerificarExistenciaPorDataInicioFim(entrada.IdUsuario, entrada.DataInicio, entrada.DataFim, idPeriodo),
                PeriodoMensagem.Datas_Abrangidas_Outro_Periodo);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            periodo.Alterar(entrada);

            _periodoRepositorio.Atualizar(periodo);

            await _uow.Commit();

            return new Saida(true, new[] { PeriodoMensagem.Periodo_Alterado_Com_Sucesso }, new PeriodoSaida(periodo));
        }

        public async Task<ISaida> ExcluirPeriodo(int idPeriodo, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idPeriodo, 0, PeriodoMensagem.Id_Periodo_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var periodo = await _periodoRepositorio.ObterPorId(idPeriodo);

            // Verifica se o período existe
            this.NotificarSeNulo(periodo, PeriodoMensagem.Id_Periodo_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o período pertece ao usuário informado.
            this.NotificarSeDiferentes(periodo.IdUsuario, idUsuario, PeriodoMensagem.Periodo_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _periodoRepositorio.Deletar(periodo);

            await _uow.Commit();

            return new Saida(true, new[] { PeriodoMensagem.Periodo_Excluido_Com_Sucesso }, new PeriodoSaida(periodo));
        }
    }
}
