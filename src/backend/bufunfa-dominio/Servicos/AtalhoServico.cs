using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class AtalhoServico : Notificavel, IAtalhoServico
    {
        private readonly IAtalhoRepositorio _atalhoRepositorio;
        private readonly IUnitOfWork _uow;

        public AtalhoServico(IAtalhoRepositorio atalhoRepositorio, IUnitOfWork uow)
        {
            _atalhoRepositorio = atalhoRepositorio;
            _uow               = uow;
        }

        public async Task<ISaida> ObterAtalhoPorId(int idAtalho, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idAtalho, 0, AtalhoMensagem.Id_Atalho_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var atalho = await _atalhoRepositorio.ObterPorId(idAtalho);

            if (atalho == null)
                return new Saida(true, new[] { AtalhoMensagem.Id_Atalho_Nao_Existe }, null);

            // Verifica se a pessoa pertece ao usuário informado.
            this.NotificarSeDiferentes(atalho.IdUsuario, idUsuario, AtalhoMensagem.Atalho_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { AtalhoMensagem.Atalho_Encontrado_Com_Sucesso }, new AtalhoSaida(atalho));
        }

        public async Task<ISaida> ObterAtalhosPorUsuario(int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstAtalhos = await _atalhoRepositorio.ObterPorUsuario(idUsuario);

            return lstAtalhos.Any()
                ? new Saida(true, new[] { AtalhoMensagem.Atalhos_Encontrados_Com_Sucesso }, lstAtalhos.Select(x => new AtalhoSaida(x)))
                : new Saida(true, new[] { AtalhoMensagem.Nenhum_Atalho_Encontrado }, null);
        }

        public async Task<ISaida> CadastrarAtalho(AtalhoEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se já existe um atalho com o mesmo título e URL informados
            this
                .NotificarSeVerdadeiro(await _atalhoRepositorio.VerificarExistenciaPorTitulo(entrada.IdUsuario, entrada.Titulo), AtalhoMensagem.Atalho_Com_Mesmo_Titulo)
                .NotificarSeVerdadeiro(await _atalhoRepositorio.VerificarExistenciaPorUrl(entrada.IdUsuario, entrada.Titulo), AtalhoMensagem.Atalho_Com_Mesma_Url);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var atalho = new Atalho(entrada);

            await _atalhoRepositorio.Inserir(atalho);

            await _uow.Commit();

            return new Saida(true, new[] { AtalhoMensagem.Atalho_Cadastrado_Com_Sucesso }, new AtalhoSaida(atalho));
        }

        public async Task<ISaida> AlterarAtalho(int idAtalho, AtalhoEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var atalho = await _atalhoRepositorio.ObterPorId(idAtalho);

            // Verifica se o atalho existe
            this.NotificarSeNulo(atalho, string.Format(AtalhoMensagem.Id_Atalho_Nao_Existe, idAtalho));

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a pessoa pertece ao usuário informado.
            this.NotificarSeDiferentes(atalho.IdUsuario, entrada.IdUsuario, AtalhoMensagem.Atalho_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe um atalho com o mesmo título e URL informados
            this
                .NotificarSeVerdadeiro(await _atalhoRepositorio.VerificarExistenciaPorTitulo(entrada.IdUsuario, entrada.Titulo, idAtalho), AtalhoMensagem.Atalho_Com_Mesmo_Titulo)
                .NotificarSeVerdadeiro(await _atalhoRepositorio.VerificarExistenciaPorUrl(entrada.IdUsuario, entrada.Url, idAtalho), AtalhoMensagem.Atalho_Com_Mesma_Url);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            atalho.Alterar(entrada);

            _atalhoRepositorio.Atualizar(atalho);

            await _uow.Commit();

            return new Saida(true, new[] { AtalhoMensagem.Atalho_Alterado_Com_Sucesso }, new AtalhoSaida(atalho));
        }

        public async Task<ISaida> ExcluirAtalho(int idAtalho, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idAtalho, 0, AtalhoMensagem.Id_Atalho_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var atalho = await _atalhoRepositorio.ObterPorId(idAtalho);

            // Verifica se o atalho existe
            this.NotificarSeNulo(atalho, AtalhoMensagem.Id_Atalho_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o atalho pertece ao usuário informado.
            this.NotificarSeDiferentes(atalho.IdUsuario, idUsuario, AtalhoMensagem.Atalho_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _atalhoRepositorio.Deletar(atalho);

            await _uow.Commit();

            return new Saida(true, new[] { AtalhoMensagem.Atalho_Excluido_Com_Sucesso }, new AtalhoSaida(atalho));
        }
    }
}
