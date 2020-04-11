using JNogueira.Bufunfa.Dominio.Comandos;
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
    public class GraficoServico : Notificavel, IGraficoServico
    {
        private readonly IPeriodoRepositorio _periodoRepositorio;
        private readonly ILancamentoRepositorio _lancamentoRepositorio;
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IParcelaRepositorio _parcelaRepositorio;

        public GraficoServico(
            IPeriodoRepositorio periodoRepositorio,
            ILancamentoRepositorio lancamentoRepositorio,
            ICategoriaRepositorio categoriaRepositorio,
            IParcelaRepositorio parcelaRepositorio)
        {
            _periodoRepositorio    = periodoRepositorio;
            _lancamentoRepositorio = lancamentoRepositorio;
            _categoriaRepositorio  = categoriaRepositorio;
            _parcelaRepositorio    = parcelaRepositorio;
        }

        public async Task<ISaida> GerarGraficoRelacaoValorCategoriaPorAno(int ano, int idCategoria, int idUsuario)
        {
            this
                .NotificarSeMenorQue(ano, 2019, "O ano informado é inválido. Informe um ano maior ou igual a 2019")
                .NotificarSeMenorOuIgualA(idCategoria, 0, CategoriaMensagem.Id_Categoria_Invalido)
                .NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            this.NotificarSeFalso(await _categoriaRepositorio.VerificarExistenciaPorId(idUsuario, idCategoria), CategoriaMensagem.Categoria_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var categoria = new CategoriaSaida(await _categoriaRepositorio.ObterPorId(idCategoria));

            var periodosPorAno = (await _periodoRepositorio.ObterPorAno(ano, idUsuario))?.Select(x => new PeriodoSaida(x));

            if (periodosPorAno?.Any() == false)
                return new Saida(true, new[] { "Nenhum período foi encontrado para o ano informado." }, null);

            var periodos = new List<PeriodoGraficoRelacaoValorCategoriaPorAnoSaida>();

            foreach(var periodo in periodosPorAno)
            {
                var periodoGrafico = await ObterPeriodoGraficoRelacaoValorCategoriaPorAnoSaida(ano, idCategoria, periodo, idUsuario);
                
                periodos.Add(periodoGrafico);
            }

            return new Saida(true, new[] { "Informações obtidas com sucesso." }, new GraficoRelacaoValorCategoriaPorAnoSaida(idUsuario, ano, categoria, periodos));
        }

        public async Task<ISaida> ObterPeriodoGraficoRelacaoValorCategoriaPorAno(int idPeriodo, int ano, int idCategoria, int idUsuario)
        {
            this
                .NotificarSeMenorQue(ano, 2019, "O ano informado é inválido. Informe um ano maior ou igual a 2019")
                .NotificarSeMenorOuIgualA(idCategoria, 0, CategoriaMensagem.Id_Categoria_Invalido)
                .NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            this.NotificarSeFalso(await _categoriaRepositorio.VerificarExistenciaPorId(idUsuario, idCategoria), CategoriaMensagem.Categoria_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var periodo = await _periodoRepositorio.ObterPorId(idPeriodo);

            this.NotificarSeNulo(periodo, PeriodoMensagem.Id_Periodo_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var periodoGrafico = await ObterPeriodoGraficoRelacaoValorCategoriaPorAnoSaida(ano, idCategoria, new PeriodoSaida(periodo), idUsuario);

            return new Saida(true, new[] { "Informações do período obtidas com sucesso." }, periodoGrafico);
        }

        private async Task<PeriodoGraficoRelacaoValorCategoriaPorAnoSaida> ObterPeriodoGraficoRelacaoValorCategoriaPorAnoSaida(int ano, int idCategoria, PeriodoSaida periodo, int idUsuario)
        {
            var dataInicioPeriodo = periodo.DataInicio.Year < ano ? new DateTime(ano, 1, 1) : periodo.DataInicio;
            var dataFimPeriodo = periodo.DataFim.Year > ano ? new DateTime(ano, 12, 31) : periodo.DataFim;

            // Obtém todos os lançamentos para um período
            var lancamentosPorPeriodo = await _lancamentoRepositorio.ObterPorPeriodo(dataInicioPeriodo, dataFimPeriodo, idUsuario);

            // Obtém somente os lançamentos do período com a categoria informada
            var lancamentosGrafico = lancamentosPorPeriodo?.Where(x => x.Categoria.IdCategoriaPai == idCategoria || x.Categoria.Id == idCategoria).Select(x => new LancamentoSaida(x));

            // Obtém os detalhes dos lançamentos onde o detalhe possui a categoria informada
            var lancamentoDetalhesGrafico = (from lancamento in lancamentosPorPeriodo
                                             from detalhe in lancamento.Detalhes
                                             where detalhe.Categoria.IdCategoriaPai == idCategoria || detalhe.Categoria.Id == idCategoria
                                             select detalhe)?.Select(x => new LancamentoDetalheSaida(x)).ToList();

            // Obtém as parcelas de faturas já lançadas, com a categoria informada
            var parcelasGrafico = (await _parcelaRepositorio.ObterPorDataLancamento(dataInicioPeriodo, dataFimPeriodo, idUsuario))?.Where(x => x.Agendamento.Categoria.IdCategoriaPai == idCategoria || x.Agendamento.IdCategoria == idCategoria).Select(x => new ParcelaSaida(x));

            return new PeriodoGraficoRelacaoValorCategoriaPorAnoSaida(
                periodo,
                lancamentosGrafico,
                lancamentoDetalhesGrafico,
                parcelasGrafico,
                dataInicioPeriodo != periodo.DataInicio ? dataInicioPeriodo : (DateTime?)null,
                dataFimPeriodo != periodo.DataFim ? dataFimPeriodo : (DateTime?)null);
        }
    }
}
