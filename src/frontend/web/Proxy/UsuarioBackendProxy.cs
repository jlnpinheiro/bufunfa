using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Proxy
{
    public partial class BackendProxy
    {
        /// <summary>
        /// Autentica um usuário
        /// </summary>
        public async Task<Saida<UsuarioAutenticacao>> AutenticarUsuario(string email, string senha)
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(new { Email = email, Senha = senha }), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<UsuarioAutenticacao>>("usuario/autenticar", MetodoHttp.POST, content, false);
            }
        }

        /// <summary>
        /// Autentica um usuário
        /// </summary>
        public async Task<Saida> AlterarSenhaUsuario(string senhaAtual, string senhaNova, string confirmarSenhaNova)
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(new { Senha = senhaAtual, SenhaNova = senhaNova, ConfirmarSenhaNova = confirmarSenhaNova }), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida>("usuario/alterar-senha", MetodoHttp.PUT, content, true);
            }
        }
    }
}
