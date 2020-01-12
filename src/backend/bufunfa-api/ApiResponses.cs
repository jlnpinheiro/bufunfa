using JNogueira.Bufunfa.Dominio.Comandos;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace JNogueira.Bufunfa.Api
{
    /// <summary>
    /// Response padrão da API para o erro HTTP 401
    /// </summary>
    public class UnauthorizedApiResponse : Saida, IExamplesProvider<Saida>
    {
        public UnauthorizedApiResponse()
        {
            this.Sucesso = false;
            this.Mensagens = new[] { "Erro 401: Acesso negado. Certifique-se que você foi autenticado." };
            this.Retorno = null;
        }

        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = false,
                Mensagens = new[] { "Erro 401: Acesso negado. Certifique-se que você foi autenticado." },
                Retorno = null
            };
        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 403
    /// </summary>
    public class ForbiddenApiResponse : Saida, IExamplesProvider<Saida>
    {
        public ForbiddenApiResponse()
        {
            this.Sucesso = false;
            this.Mensagens = new[] { "Erro 403: Acesso negado. Você não tem permissão de acesso para essa funcionalidade." };
            this.Retorno = null;
        }

        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = false,
                Mensagens = new[] { "Erro 403: Acesso negado. Você não tem permissão de acesso para essa funcionalidade." },
                Retorno = null
            };
        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 404
    /// </summary>
    public class NotFoundApiResponse : Saida, IExamplesProvider<Saida>
    {
        public NotFoundApiResponse()
        {
            this.Sucesso = false;
            this.Mensagens = new[] { "Erro 404: O endereço não encontrado." };
            this.Retorno = null;
        }

        public NotFoundApiResponse(string path)
        {
            this.Sucesso = false;
            this.Mensagens = new[] { $"Erro 404: O endereço \"{path}\" não foi encontrado." };
            this.Retorno = null;
        }

        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = false,
                Mensagens = new[] { "Erro 404: O endereço não encontrado." },
                Retorno = null
            };
        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 415
    /// </summary>
    public class UnsupportedMediaTypeApiResponse : Saida, IExamplesProvider<Saida>
    {
        public UnsupportedMediaTypeApiResponse()
        {
            this.Sucesso = false;
            this.Mensagens = new[] { "Erro 415: O tipo de requisição  não é suportado pela API." };
            this.Retorno = null;
        }

        public UnsupportedMediaTypeApiResponse(string requestContentType)
        {
            this.Sucesso = false;
            this.Mensagens = new[] { $"Erro 415: O tipo de requisição \"{requestContentType}\" não é suportado pela API." };
            this.Retorno = null;
        }

        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = false,
                Mensagens = new[] { "Erro 415: O tipo de requisição não é suportado pela API." },
                Retorno = null
            };
        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 400
    /// </summary>
    public class BadRequestApiResponse : Saida, IExamplesProvider<Saida>
    {
        public BadRequestApiResponse()
        {
            this.Sucesso = false;
            this.Mensagens = new[] { "O campo X é obrigatório e não foi informado.", "O campo Y é obrigatório e não foi informado." };
            this.Retorno = null;
        }

        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = false,
                Mensagens = new[] { "O campo X é obrigatório e não foi informado.", "O campo Y é obrigatório e não foi informado." },
                Retorno = null
            };
        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 500
    /// </summary>
    public class InternalServerErrorApiResponse : Saida, IExamplesProvider<Saida>
    {
        public InternalServerErrorApiResponse()
        {
            this.Sucesso = false;
            this.Mensagens = new[] { "Ocorreu um erro inesperado." };
            this.Retorno = null;
        }

        public InternalServerErrorApiResponse(Exception exception)
        {
            if (exception == null)
                return;

            this.Sucesso = false;
            this.Mensagens = new[] { exception.GetBaseException().Message };
            this.Retorno = new
            {
                Exception = exception.Message,
                BaseException = exception.GetBaseException().Message,
                exception.Source
            };
        }

        public Saida GetExamples()
        {
            try
            {
                var i = 1;

                try
                {
                    i /= 0;
                }
                catch (Exception ex1)
                {
                    throw new Exception("Ocorreu um erro inesperado.", ex1);
                }

                return null;
            }
            catch (Exception ex2)
            {
                return new InternalServerErrorApiResponse(ex2);
            }
        }
    }
}
