using JNogueira.Bufunfa.Web.Models;
using JNogueira.Bufunfa.Web.Proxy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace JNogueira.Bufunfa.Web.Helpers
{
    /// <summary>
    /// Classe utiliza para a renderização de alguns códigos HTML customizados
    /// </summary>
    public class CustomHtmlHelper
    {
        private readonly BackendProxy _proxy;
        private readonly IWebHostEnvironment _environment;

        public CustomHtmlHelper(BackendProxy proxy, IWebHostEnvironment environment)
        {
            _proxy = proxy;
            _environment = environment;
        }

        public HtmlString CriarTagLinkCssPorOS(string path)
        {
            return new HtmlString($"<link href=\"{(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? path : $"file://{_environment.ContentRootPath}/wwwroot/{path}")}\" rel=\"stylesheet\" type=\"text/css\" />");
        }

        public HtmlString CriarTagImgPorOS(string path)
        {
            return new HtmlString($"<img src=\"{(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? path : $"file://{_environment.ContentRootPath}/wwwroot/{path}")}\" />");
        }

        public static HtmlString IconeCampoObrigatorio()
        {
            return new HtmlString("<span class=\"fa fa-exclamation-circle\" style=\"color:#D33724;\"></span>");
        }

        public static HtmlString CriarTreeViewCategoria(IEnumerable<Categoria> categorias)
        {
            var html = new StringBuilder("<ul>");

            html.Append("<li data-jstree='{ \"icon\" : \"fa fa-folder kt-font-success\" }' class=\"jstree-open\" id=\"" + int.MaxValue + "\">CRÉDITO<ul>");

            foreach(var categoria in categorias.Where(x => x.CategoriaPai == null && x.ObterTipo() == TipoCategoria.Credito).OrderBy(x => x.Nome))
            {
                html.Append("<li data-jstree='{\"type\":" + (categoria.CategoriasFilha != null && categoria.CategoriasFilha.Any() ? "\"pai-credito\"" : "\"folha-credito\"") + "}' class=\"jstree-open\" id=\"" + categoria.Id + "\">" + categoria.Nome + " " + CriarArvorePorCategoria(categoria) + "</li>");
            }

            html.Append("</ul></li>");

            html.Append("<li data-jstree='{ \"icon\" : \"fa fa-folder kt-font-danger\" }' class=\"jstree-open\" id=\"" + int.MinValue + "\">DÉBITO<ul>");

            foreach (var categoria in categorias.Where(x => x.CategoriaPai == null && x.ObterTipo() == TipoCategoria.Debito).OrderBy(x => x.Nome))
            {
                html.Append("<li data-jstree='{\"type\":" + (categoria.CategoriasFilha != null && categoria.CategoriasFilha.Any() ? "\"pai-debito\"" : "\"folha-debito\"") + "}' class=\"jstree-open\" id=\"" + categoria.Id + "\">" + categoria.Nome + " " + CriarArvorePorCategoria(categoria) + "</li>");
            }

            html.Append("</ul></li>");

            html.Append("</ul>");

            return new HtmlString(html.ToString());
        }

        private static HtmlString CriarArvorePorCategoria(Categoria categoria)
        {
            if (categoria.CategoriasFilha == null)
                return new HtmlString(string.Empty);

            var tipo = categoria.ObterTipo() == TipoCategoria.Credito ? "credito" : "debito";

            var html = new StringBuilder("<ul>");

            foreach (var categoriaFilha in categoria.CategoriasFilha.OrderBy(x => x.Nome))
            {
                html.Append("<li data-jstree='{\"type\":" + (categoriaFilha.CategoriasFilha != null && categoriaFilha.CategoriasFilha.Any() ? "\"pai-" + tipo + "\"" : "\"folha-" + tipo + "\"") + "}' class=\"jstree-open\" id=\"" + categoriaFilha.Id + "\">" + categoriaFilha.Nome + "</li>");
            }

            html.Append("</ul>");

            return new HtmlString(html.ToString());
        }

        public HtmlString SelectCategorias(
            string id,
            string cssClass,
            string valorSelecionado,
            bool permitirSelecaoPai = false,
            string atributosHtml = "style=\"width: 100%;\"")
        {
            var saida = _proxy.ObterCategorias().Result;

            if (!saida.Sucesso)
                return new HtmlString("<div class=\"kt-font-warning kt-font-bolder text-center\">Não foi possível obter as categorias.</div>");

            if (saida.Sucesso && saida.Retorno == null)
                return new HtmlString("<div class=\"kt-font-warning kt-font-bolder text-center\">Nenhuma categoria cadastrada.</div>");

            var atribId = !string.IsNullOrEmpty(id) ? $"id=\"{id}\" name=\"{id}\"" : string.Empty;

            var html = new StringBuilder($"<select {atribId} class=\"{cssClass}\"");

            if (!string.IsNullOrEmpty(atributosHtml))
                html.Append(atributosHtml);

            html.AppendLine(">");

            html.AppendLine("<option></option>");

            var lst = new List<Categoria>();

            if (saida.Sucesso && saida.Retorno != null)
                lst = new List<Categoria>(saida.Retorno);

            if (permitirSelecaoPai)
            {
                lst.Add(new Categoria
                {
                    Id = -1,
                    Tipo = "C",
                    Nome = "CRÉDITO",
                    Caminho = "CRÉDITO",
                    CategoriasFilha = lst?.Where(x => x.Tipo == "C").ToArray()
                });

                lst.Add(new Categoria
                {
                    Id = -2,
                    Tipo = "D",
                    Nome = "DÉBITO",
                    Caminho = "DÉBITO",
                    CategoriasFilha = lst?.Where(x => x.Tipo == "D").ToArray()
                });

                foreach (var categoria in lst.OrderBy(x => x.Tipo).ThenBy(x => x.Caminho))
                {
                    html.AppendLine($"<option value=\"{categoria.Id}\" " +
                        $"data-tipo=\"{categoria.Tipo}\" " +
                        $"data-caminho=\"{categoria.Caminho}\" " +
                        $"data-possui-filhas=\"{(categoria.CategoriasFilha != null && categoria.CategoriasFilha.Any() ? "1" : "0")}\" " +
                        $"{(valorSelecionado == categoria.Id.ToString() ? " selected" : string.Empty)} " +
                        $"{(categoria.CategoriasFilha != null && categoria.CategoriasFilha.Any() && !permitirSelecaoPai ? "disabled" : string.Empty)}>" +
                        $"{categoria.Caminho}</option>");
                }
            }
            else
            {
                foreach (var categoria in lst.Where(x => x.CategoriasFilha == null || !x.CategoriasFilha.Any()).OrderBy(x => x.Tipo).ThenBy(x => x.Caminho))
                {
                    html.AppendLine($"<option value=\"{categoria.Id}\" " +
                        $"data-tipo=\"{categoria.Tipo}\" " +
                        $"data-caminho=\"{categoria.Caminho}\" " +
                        $"{(valorSelecionado == categoria.Id.ToString() ? " selected" : string.Empty)}>" +
                        $"{categoria.Caminho}</option>");
                }
            }

            html.Append("</select>");

            return new HtmlString(html.ToString());
        }

        public HtmlString SelectContasCartoesCredito(
            string id,
            string cssClass,
            string valorSelecionado,
            TipoItemSelectContasCartoesCredito tipoItem = TipoItemSelectContasCartoesCredito.ContasCartoesCredito,
            string atributosHtml = "style=\"width: 100%;\"")
        {
            var contas = new List<Conta>();
            var cartoesCredito = new List<CartaoCredito>();

            Saida<IEnumerable<Conta>> contasSaida = null;
            Saida<IEnumerable<CartaoCredito>> cartoesCreditoSaida = null;

            switch (tipoItem)
            {
                case TipoItemSelectContasCartoesCredito.ContasCartoesCredito:
                    contasSaida = _proxy.ObterContas().Result;

                    if (contasSaida.Sucesso && contasSaida.Retorno != null)
                        contas.AddRange(contasSaida.Retorno.Where(x => x.CodigoTipo != (int)TipoConta.RendaVariavel));

                    cartoesCreditoSaida = _proxy.ObterCartoesCredito().Result;

                    if (cartoesCreditoSaida.Sucesso && cartoesCreditoSaida.Retorno != null)
                        cartoesCredito.AddRange(cartoesCreditoSaida.Retorno);

                    if (contas.Count == 0 && cartoesCredito.Count == 0)
                        return new HtmlString("<div class=\"kt-font-warning kt-font-bolder text-center\">Nenhuma conta ou cartão de crédito encontrados.</div>");

                    break;
                case TipoItemSelectContasCartoesCredito.SomenteContas:
                    contasSaida = _proxy.ObterContas().Result;

                    if (contasSaida.Sucesso && contasSaida.Retorno != null)
                        contas.AddRange(contasSaida.Retorno.Where(x => x.CodigoTipo != (int)TipoConta.RendaVariavel));

                    if (contas.Count == 0)
                        return new HtmlString("<div class=\"kt-font-warning kt-font-bolder text-center\">Nenhuma conta encontrada.</div>");

                    break;
                case TipoItemSelectContasCartoesCredito.SomenteCartoesCredito:
                    cartoesCreditoSaida = _proxy.ObterCartoesCredito().Result;

                    if (cartoesCreditoSaida.Sucesso && cartoesCreditoSaida.Retorno != null)
                        cartoesCredito.AddRange(cartoesCreditoSaida.Retorno);

                    if (cartoesCredito.Count == 0)
                        return new HtmlString("<div class=\"kt-font-warning kt-font-bolder text-center\">Nenhum cartão de crédito encontrado.</div>");

                    break;
            }

            var atribId = !string.IsNullOrEmpty(id) ? $"id=\"{id}\" name=\"{id}\"" : string.Empty;

            var html = new StringBuilder($"<select {atribId} class=\"{cssClass}\" data-tipo-item=\"{(int)tipoItem}\"");

            if (!string.IsNullOrEmpty(atributosHtml))
                html.Append(atributosHtml);

            html.AppendLine(">");

            html.AppendLine("<option></option>");

            foreach (var conta in contas)
            {
                html.AppendLine($"<option value=\"{conta.Id}\" " +
                    $"data-tipo=\"{conta.CodigoTipo}\" " +
                    $"data-saldo-atual=\"{conta.ValorSaldoAtual}\" " +
                    $"data-banco=\"{conta.NomeInstituicao}\" " +
                    $"data-cor=\"{conta.ObterCorPorTipoConta()}\" " +
                    $"{(valorSelecionado == conta.Id.ToString() ? " selected" : string.Empty)}>" +
                    $"{conta.Nome}</option>");
            }

            foreach (var cartao in cartoesCredito)
            {
                html.AppendLine($"<option value=\"{cartao.Id}\" " +
                    "data-tipo=\"CC\" " +
                    $"data-limite-disponivel=\"{cartao.ValorLimiteDisponivel}\" " +
                    $"data-dia-vencimento-fatura=\"{cartao.DiaVencimentoFatura.ToString().PadLeft(2, '0')}\" " +
                    $"{(valorSelecionado == cartao.Id.ToString() ? " selected" : string.Empty)}>" +
                    $"{cartao.Nome}</option>");
            }

            html.Append("</select>");

            return new HtmlString(html.ToString());
        }
    }

    public enum TipoItemSelectContasCartoesCredito
    {
        ContasCartoesCredito = 1,
        SomenteContas = 2,
        SomenteCartoesCredito = 3
    }
}
