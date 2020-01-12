using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace JNogueira.Bufunfa.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Obtém do token JWT, o ID do usuário
        /// </summary>
        public int ObterIdUsuarioClaim() => Convert.ToInt32(User.Claims.First(x => x.Type == "IdUsuario").Value);

        /// <summary>
        /// Obtém do token JWT, o e-mail do usuário
        /// </summary>
        public string ObterEmailUsuarioClaim() => User.Claims.ElementAt(1).Value;
    }
}
