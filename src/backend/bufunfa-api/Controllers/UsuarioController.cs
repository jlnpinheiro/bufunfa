using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Bufunfa.Infraestrutura;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Api.Controllers
{
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Erro não tratado encontrado. (Internal Server Error)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Notificações existentes. (Bad Request)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Endereço não encontrado. (Not found)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponse))]
    [SwaggerTag("Permite a gestão e consulta dos usuários.")]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioServico _usuarioServico;

        public UsuarioController(IUsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        /// <summary>
        /// Realiza a autenticação do usuário, a partir do e-mail e senha informados.
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [Consumes("application/json")]
        [Route("usuario/autenticar")]
        [SwaggerOperation(Description = "Caso a autenticação ocorra com sucesso, um token JWT é gerado e retornado.")]
        [SwaggerRequestExample(typeof(AutenticarUsuarioViewModel), typeof(AutenticarUsuarioRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Caso o usuário seja autenticado com sucesso, o token JWT é retornado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AutenticarUsuarioResponseExemplo))]
        public async Task<IActionResult> Autenticar(
            [FromBody, SwaggerParameter("E-mail e senha do usuário.", Required = true)] AutenticarUsuarioViewModel model,
            [FromServices] ConfigurationHelper configHelper /*FromServices: resolvidos via mecanismo de injeção de dependências do ASP.NET Core*/)
        {
            var autenticarComando = new AutenticarUsuarioEntrada(model.Email, model.Senha);

            var comandoSaida = await _usuarioServico.Autenticar(autenticarComando);

            if (!comandoSaida.Sucesso)
                return new ApiResult(comandoSaida);

            var usuario = (UsuarioSaida)comandoSaida.Retorno;

            var dataCriacaoToken = DateTime.Now;
            var dataExpiracaoToken = dataCriacaoToken + TimeSpan.FromHours(configHelper.JwtTokenConfig.ExpiracaoEmHoras);

            return CriarResponseTokenJwt(usuario, dataCriacaoToken, dataExpiracaoToken, configHelper);
        }

        /// <summary>
        /// Realiza a alteração da senha de acesso de um usuário
        /// </summary>
        [Authorize("Bearer")]
        [HttpPut]
        [Consumes("application/json")]
        [Route("usuario/alterar-senha")]
        [SwaggerRequestExample(typeof(AlterarSenhaUsuarioViewModel), typeof(AlterarSenhaUsuarioRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Senha de acesso alterada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarSenhaUsuarioResponseExemplo))]
        public async Task<IActionResult> AlterarSenha(
            [FromBody, SwaggerParameter("Senha atual, nova senha e confirmação da nova senha.", Required = true)] AlterarSenhaUsuarioViewModel model)
        {
            var entrada = new AlterarSenhaUsuarioEntrada(base.ObterEmailUsuarioClaim(), model.Senha, model.SenhaNova, model.ConfirmarSenhaNova);

            return new ApiResult(await _usuarioServico.AlterarSenha(entrada));
        }

        private IActionResult CriarResponseTokenJwt(UsuarioSaida usuario, DateTime dataCriacaoToken, DateTime dataExpiracaoToken, ConfigurationHelper configHelper)
        {
            var identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.Nome),
                    // Geração de claims. No contexto desse sistema, claims não precisaram ser criadas.
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Email),
                        new Claim("Nome", usuario.Nome),
                        new Claim("IdUsuario", usuario.Id.ToString())
                    }
                );

            var jwtHandler = new JwtSecurityTokenHandler();

            // Configuração realizada, seguindo o artigo "ASP.NET Core 2.0: autenticação em APIs utilizando JWT" 
            // (https://medium.com/@renato.groffe/asp-net-core-2-0-autentica%C3%A7%C3%A3o-em-apis-utilizando-jwt-json-web-tokens-4b1871efd)

            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configHelper.JwtTokenConfig.SecurityKey));

            var securityToken = jwtHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = configHelper.JwtTokenConfig.Issuer,
                Audience = configHelper.JwtTokenConfig.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                Subject = identity,
                NotBefore = dataCriacaoToken,
                Expires = dataExpiracaoToken
            });

            // Cria o token JWT em formato de string
            var jwtToken = jwtHandler.WriteToken(securityToken);

            return new ApiResult(new Saida(true, new[] { UsuarioMensagem.Usuario_Autenticado_Com_Sucesso }, new
            {
                DataCriacaoToken = dataCriacaoToken.ToString("dd/MM/yyyy HH:mm:ss"),
                DataExpiracaoToken = dataExpiracaoToken.ToString("dd/MM/yyyy HH:mm:ss"),
                Token = jwtToken,
            }));
        }
    }
}