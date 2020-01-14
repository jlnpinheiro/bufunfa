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
    public class CategoriaServico : Notificavel, ICategoriaServico
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;
        private readonly IUnitOfWork _uow;

        public CategoriaServico(
            ICategoriaRepositorio categoriaRepositorio,
            IUnitOfWork uow)
        {
            _categoriaRepositorio = categoriaRepositorio;
            _uow                  = uow;
        }

        public async Task<ISaida> ObterCategoriaPorId(int idCategoria, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idCategoria, 0, CategoriaMensagem.Id_Categoria_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var categoria = await _categoriaRepositorio.ObterPorId(idCategoria);

            if (categoria == null)
                return new Saida(true, new[] { CategoriaMensagem.Id_Categoria_Nao_Existe }, null);

            // Verifica se a categoria pertece ao usuário informado.
            if (categoria.IdUsuario.HasValue)
                this.NotificarSeDiferentes(categoria.IdUsuario.Value, idUsuario, CategoriaMensagem.Categoria_Nao_Pertence_Usuario);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { CategoriaMensagem.Categoria_Encontrada_Com_Sucesso }, new CategoriaSaida(categoria));
        }

        public async Task<ISaida> ObterCategoriasPorUsuario(int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var lstCategorias = await _categoriaRepositorio.ObterPorUsuario(idUsuario);

            return lstCategorias.Any()
                ? new Saida(true, new[] { CategoriaMensagem.Categorias_Encontradas_Com_Sucesso }, lstCategorias.OrderBy(x => x.ObterCaminho()).Select(x => new CategoriaSaida(x)))
                : new Saida(true, new[] { CategoriaMensagem.Nenhuma_categoria_encontrada }, null);
        }

        public async Task<ISaida> ProcurarCategorias(ProcurarCategoriaEntrada entrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return entrada.Invalido
                ? new Saida(false, entrada.Mensagens, null)
                : await _categoriaRepositorio.Procurar(entrada);
        }

        public async Task<ISaida> CadastrarCategoria(CategoriaEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se já existe uma categoria com o mesmo nome e mesmo tipo
            this.NotificarSeVerdadeiro(await _categoriaRepositorio.VerificarExistenciaPorNomeTipo(entrada.IdUsuario, entrada.Nome, entrada.Tipo, entrada.IdCategoriaPai), CategoriaMensagem.Categoria_Com_Mesmo_Nome_Tipo);

            if (entrada.IdCategoriaPai.HasValue)
            {
                var categoriaPai = await _categoriaRepositorio.ObterPorId(entrada.IdCategoriaPai.Value);

                // Verifica se a categoria pai existe
                this.NotificarSeNulo(categoriaPai, string.Format(CategoriaMensagem.Categoria_Pai_Nao_Existe, entrada.IdCategoriaPai.Value));

                if (categoriaPai != null)
                {
                    // Verificar se o tipo da categoria é igual ao tipo da categoria pai
                    this.NotificarSeDiferentes(entrada.Tipo, categoriaPai.Tipo, CategoriaMensagem.Tipo_Nao_Pode_Ser_Diferente_Tipo_Categoria_Pai);
                }
            }

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var categoria = new Categoria(entrada);

            await _categoriaRepositorio.Inserir(categoria);

            await _uow.Commit();

            return new Saida(true, new[] { CategoriaMensagem.Categoria_Cadastrada_Com_Sucesso }, new CategoriaSaida(await _categoriaRepositorio.ObterPorId(categoria.Id)));
        }

        public async Task<ISaida> AlterarCategoria(int idCategoria, CategoriaEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var categoria = await _categoriaRepositorio.ObterPorId(idCategoria);

            // Verifica se a categoria existe
            this.NotificarSeNulo(categoria, CategoriaMensagem.Id_Categoria_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a categoria pertece ao usuário informado.
            this.NotificarSeDiferentes(categoria.IdUsuario, entrada.IdUsuario, CategoriaMensagem.Categoria_Alterar_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe uma categoria com o mesmo nome e mesmo tipo
            this.NotificarSeVerdadeiro(await _categoriaRepositorio.VerificarExistenciaPorNomeTipo(entrada.IdUsuario, entrada.Nome, entrada.Tipo, entrada.IdCategoriaPai, idCategoria), CategoriaMensagem.Categoria_Com_Mesmo_Nome_Tipo);

            if (entrada.IdCategoriaPai.HasValue)
            {
                var categoriaPai = await _categoriaRepositorio.ObterPorId(entrada.IdCategoriaPai.Value);

                // Verifica se a categoria pai existe
                this.NotificarSeNulo(categoriaPai, CategoriaMensagem.Categoria_Pai_Nao_Existe);

                if (categoriaPai != null)
                {
                    // Verificar se o tipo da categoria é igual ao tipo da categoria pai
                    this.NotificarSeDiferentes(entrada.Tipo, categoriaPai.Tipo, CategoriaMensagem.Tipo_Nao_Pode_Ser_Diferente_Tipo_Categoria_Pai);
                }
            }

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            categoria.Alterar(entrada);

            _categoriaRepositorio.Atualizar(categoria);

            await _uow.Commit();

            return new Saida(true, new[] { CategoriaMensagem.Categoria_Alterada_Com_Sucesso }, new CategoriaSaida(await _categoriaRepositorio.ObterPorId(categoria.Id)));
        }

        public async Task<ISaida> ExcluirCategoria(int idCategoria, int idUsuario)
        {
            this.NotificarSeMenorOuIgualA(idCategoria, 0, CategoriaMensagem.Id_Categoria_Invalido);
            this.NotificarSeMenorOuIgualA(idUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var categoria = await _categoriaRepositorio.ObterPorId(idCategoria);

            // Verifica se a categoria existe
            this.NotificarSeNulo(categoria, CategoriaMensagem.Id_Categoria_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se a categoria pertece ao usuário informado.
            this.NotificarSeDiferentes(categoria.IdUsuario, idUsuario, CategoriaMensagem.Categoria_Excluir_Nao_Pertence_Usuario);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verificar se a categoria tem filhas. Se for pai, obrigar a exclusão de todas as filhas primeiro
            this.NotificarSeVerdadeiro(categoria.VerificarSePai(), CategoriaMensagem.Categoria_Pai_Nao_Pode_Ser_Excluida);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _categoriaRepositorio.Deletar(categoria);

            await _uow.Commit();

            return new Saida(true, new[] { CategoriaMensagem.Categoria_Excluida_Com_Sucesso }, new CategoriaSaida(categoria));
        }
    }
}