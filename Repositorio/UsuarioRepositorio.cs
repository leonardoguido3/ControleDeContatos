using ControleDeContatos.Data;
using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeContatos.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        //Injetor de dependencia do Contexto e Email
        private readonly DataContext _dataContext;
        private readonly IEmail _email;

        //Construtor
        public UsuarioRepositorio(DataContext dataContext, IEmail email)
        {
            this._dataContext = dataContext;
            _email = email;
        }

        //Função para adicionar um usuario, setando na adição a data e chamando o metodo gerarhash para encriptografar a senha
        public UsuarioModel Adicionar(UsuarioModel usuario)
        {
            usuario.DataCadastro = DateTime.Now;
            usuario.SetSenhaHash();
            _dataContext.Usuarios.Add(usuario);
            _dataContext.SaveChanges();
            return usuario;
        }

        //Função para alterar senha de um usuario, tendo tratativas, verificando se a senha confere, se não é igual a antiga, e se está vindo com algum conteudo, apos validar, é alterado e enviado email ao usuario
        public UsuarioModel AlterarSenha(AlterarSenhaModel alterarSenhaModel)
        {
            UsuarioModel usuarioDB = ListarPorId(alterarSenhaModel.Id);
            if (usuarioDB == null) throw new Exception("Houve um erro na atualização da senha, o usuario não foi encontrado!");

            if (!usuarioDB.SenhaValida(alterarSenhaModel.SenhaAtual)) throw new Exception("Senha atual não confere, por favor, verifique os dados!");

            if (usuarioDB.SenhaValida(alterarSenhaModel.novaSenha)) throw new Exception("A nova senha precisa ser diferente da atual!");

            usuarioDB.SetNovaSenha(alterarSenhaModel.novaSenha);
            usuarioDB.DataAtualizacao = DateTime.Now;

            _dataContext.Usuarios.Update(usuarioDB);
            _dataContext.SaveChanges();
            string mensagem = $"Olá, este é um email de informação. Sua senha foi alterada em {DateTime.Now}, se não foi você favor entrar em contato com o Administrador!";
            _email.Enviar(usuarioDB.Email, "CRM - Max Net | Nova Senha", mensagem);

            return usuarioDB;

        }

        //Função de deleção retornando um booleano, onde é realizado tratativas para que o usuario a ser deletado seja válido, após é realizado o delete
        public bool Apagar(int id)
        {
            UsuarioModel usuarioDB = ListarPorId(id);
            if (usuarioDB == null) throw new Exception("Houve um erro na exclusão do usuário!");
            _dataContext.Remove(usuarioDB);
            _dataContext.SaveChanges();

            return true;
        }

        //Função de atualização de cadastro, realiza tratativa dos dados, e adiciona a data atual no objeto de data da ultima atualização
        public UsuarioModel Atualizar(UsuarioModel usuario)
        {
            UsuarioModel usuarioDB = ListarPorId(usuario.Id);

            if (usuarioDB == null) throw new Exception("Houve um erro na atualização do usuario!");

            usuarioDB.Nome = usuario.Nome;
            usuarioDB.Login = usuario.Login;
            usuarioDB.Email = usuario.Email;
            usuarioDB.DataAtualizacao = DateTime.Now;

            _dataContext.Usuarios.Update(usuarioDB);
            _dataContext.SaveChanges();
            return usuarioDB;

        }

        //Função de buscar email e login, consultamos o primeiro ou unico dados que tenha o login e email que batem, para a recuperação da senha
        public UsuarioModel BuscarPorEmailELogin(string email, string login)
        {
            return _dataContext.Usuarios.FirstOrDefault(u => u.Login == login || u.Email.ToUpper() == email.ToUpper());
        }

        //Função de buscar por login, este método valida o usuario de acesso com o do banco de dados
        public UsuarioModel BuscarPorLogin(string login)
        {
            return _dataContext.Usuarios.FirstOrDefault(u => u.Login == login);
        }

        //Este metodo busca todos os usuarios, incluindo em sua listagem os contatos cadastrados por eles
        public List<UsuarioModel> BuscarTodos()
        {
            return _dataContext.Usuarios
                .Include(x => x.Contatos)
                .ToList();
        }

        //Função Listar por id, lista o primeiro ou unico, comparando o ID passado com o do banco de dados
        public UsuarioModel ListarPorId(int id)
        {
            return _dataContext.Usuarios.FirstOrDefault(u => u.Id == id);
        }
    }
}
