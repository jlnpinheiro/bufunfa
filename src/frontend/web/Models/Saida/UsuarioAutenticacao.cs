using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados do processo de autenticação de um usuário
    /// </summary>
    public class UsuarioAutenticacao
    {
        /// <summary>
        /// Data da criação do token
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy HH:mm:ss")]
        public DateTime? DataCriacaoToken { get; set; }

        /// <summary>
        /// Data de expiração do token
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy HH:mm:ss")]
        public DateTime? DataExpiracaoToken { get; set; }

        /// <summary>
        /// Token JWT
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Extrai o nome do usuário do token JWT
        /// </summary>
        public string ObterNomeUsuario() => this.ObterClaims().FirstOrDefault(x => x.Type == "Nome")?.Value;

        /// <summary>
        /// Extrai os claims do token JWT
        /// </summary>
        public IEnumerable<Claim> ObterClaims()
        {
            return string.IsNullOrEmpty(this.Token)
                ? new List<Claim>()
                : this.ExtrairClaims(this.Token);
        }

        /// <summary>
        /// Extrái as claims de um token JWT
        /// </summary>
        public IEnumerable<Claim> ExtrairClaims(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
                return new List<Claim>();

            var jwtHandler = new JwtSecurityTokenHandler();

            if (!jwtHandler.CanReadToken(jwtToken))
                return new List<Claim>();

            var jwtSecurityToken = jwtHandler.ReadJwtToken(jwtToken);

            return jwtSecurityToken?.Claims;
        }
    }
}
