using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace JNogueira.Bufunfa.Web.Helpers
{
    /// <summary>
    /// Classe utilizada para extrair informações do cookie de autenticação
    /// </summary>
    public class CookieHelper
    {
        private readonly HttpContext _context;

        public CookieHelper(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor.HttpContext;
        }

        /// <summary>
        /// Obtém do ID do usuário
        /// </summary>
        public int ObterIdUsuario() => Convert.ToInt32(_context.User.Claims.First(x => x.Type == "IdUsuario").Value);

        /// <summary>
        /// Obtém o nome do usuário
        /// </summary>
        public string ObterNomeUsuario() => _context.User.Claims.First(x => x.Type == "Nome").Value;

        /// <summary>
        /// Obtém o primeiro nome do usuário
        /// </summary>
        public string ObterPrimeiroNomeUsuario() => ObterNomeUsuario().Split(" ".ToCharArray())[0];

        /// <summary>
        /// Obtém o e-mail do usuário
        /// </summary>
        public string ObterEmailUsuario() => _context.User.Claims.Where(x => x.Type == "unique_name")?.ElementAt(1)?.Value;

        /// <summary>
        /// Obtém o token JWT associado ao cookie
        /// </summary>
        public string ObterTokenJwt() => _context.User.Claims.FirstOrDefault(x => x.Type == "jwtToken")?.Value;
    }
}
