using JNogueira.Bufunfa.Web.Filters;
using JNogueira.Bufunfa.Web.Proxy;
using JNogueira.Bufunfa.Web.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Controllers
{
    [Authorize]
    public class UsuarioController : BaseController
    {
        public UsuarioController(BackendProxy proxy)
            : base(proxy)
        {

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return User.Identity.IsAuthenticated
                ? (ActionResult)RedirectToAction("Index", "Dashboard")
                : View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("usuario/login")]
        [FeedbackExceptionFilter("Ocorreu um erro na tentativa de efetuar login.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> Login(string email, string senha, bool permanecerLogado)
        {
            var saida = await _proxy.AutenticarUsuario(email, senha);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível efetuar o login.", saida.Mensagens));

            // Cria o cookie de autenticação

            var claims = new List<Claim>(saida.Retorno.ObterClaims());
            claims.Add(new Claim("jwtToken", saida.Retorno.Token));

            var userIdentity = new ClaimsIdentity(
                new GenericIdentity(saida.Retorno.ObterNomeUsuario()),
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme,
                null,
                null);

            var principal = new ClaimsPrincipal(userIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = permanecerLogado,
                ExpiresUtc = saida.Retorno.DataExpiracaoToken
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);

            return new JsonResult(new { saida.Retorno.Token });
        }

        [Authorize]
        [HttpGet]
        [Route("usuario/alterar-senha")]
        public IActionResult AlterarSenha()
        {
            return PartialView("AlterarSenha");
        }

        [Authorize]
        [HttpPost]
        [Route("usuario/alterar-senha")]
        public async Task<IActionResult> AlterarSenha(string senhaAtual, string novaSenha, string confirmarNovaSenha)
        {
            var saida = await _proxy.AlterarSenhaUsuario(senhaAtual, novaSenha, confirmarNovaSenha);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar a senha do usuário.", saida.Mensagens));

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), new string[] { "Será necessário que você faça login novamente. Ao clicar no botão \"OK\" você será redirecionado para a tela de login." }, tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaLogin));
        }

        [Authorize]
        [HttpPost]
        [Route("usuario/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return new EmptyResult();
        }
    }
}
