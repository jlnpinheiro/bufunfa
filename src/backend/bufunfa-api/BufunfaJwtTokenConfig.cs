using JNogueira.Bufunfa.Infraestrutura;
using Microsoft.IdentityModel.Tokens;

namespace JNogueira.Bufunfa.Api
{
    /// <summary>
    /// Classe que armazena as configurações do token JWT
    /// </summary>
    public class BufunfaJwtTokenConfig
    {
        // A propriedade Key, à qual será vinculada uma instância da classe SecurityKey (namespace Microsoft.IdentityModel.Tokens) 
        // armazenando a chave de criptografia utilizada na criação de tokens;
        public SecurityKey Key { get; }

        // A propriedade SigningCredentials, que receberá um objeto baseado em uma classe também chamada SigningCredentials (namespace Microsoft.IdentityModel.Tokens). 
        // Esta referência conterá a chave de criptografia e o algoritmo de segurança empregados na geração de assinaturas digitais para tokens
        public SigningCredentials SigningCredentials { get; }

        private readonly ConfigurationHelper _configHelper;

        public BufunfaJwtTokenConfig(ConfigurationHelper configHelper)
        {
            Key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes("F7DEC0E0-4F78-4ED4-A89A-4EF3C6B82374"));

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            _configHelper = configHelper;
        }
    }
}
