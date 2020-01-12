using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class LancamentoAnexoSaidaExemplo : LancamentoAnexoSaida
    {
        public LancamentoAnexoSaidaExemplo()
            : base(1, 1, "1gF8wE6OVfCnghANI70A-gh9rXc-jNGob", "Comprovante", "comprovante.pdf")
        {

        }
    }

    public class CadastrarAnexoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoAnexoMensagem.Anexo_Cadastrado_Com_Sucesso },
                Retorno = new LancamentoAnexoSaidaExemplo()
            };
        }
    }    

    public class ExcluirAnexoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoAnexoMensagem.Anexo_Excluido_Com_Sucesso },
                Retorno = new LancamentoAnexoSaidaExemplo()
            };
        }
    }
}
