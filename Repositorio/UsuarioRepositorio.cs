using ControleDeContatos.Data;
using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeContatos.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly DataContext _dataContext;
        private readonly IEmail _email;

        public UsuarioRepositorio(DataContext dataContext, IEmail email)
        {
            this._dataContext = dataContext;
            _email = email;
        }

        public UsuarioModel Adicionar(UsuarioModel usuario)
        {
            usuario.DataCadastro = DateTime.Now;
            usuario.SetSenhaHash();
            _dataContext.Usuarios.Add(usuario);
            _dataContext.SaveChanges();
            return usuario;
        }

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

        public bool Apagar(int id)
        {
            UsuarioModel usuarioDB = ListarPorId(id);
            if (usuarioDB == null) throw new Exception("Houve um erro na exclusão do usuário!");
            _dataContext.Remove(usuarioDB);
            _dataContext.SaveChanges();

            return true;
        }

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

        public UsuarioModel BuscarPorEmailELogin(string email, string login)
        {
            return _dataContext.Usuarios.FirstOrDefault(u => u.Login == login || u.Email.ToUpper() == email.ToUpper());
        }

        public UsuarioModel BuscarPorLogin(string login)
        {
            return _dataContext.Usuarios.FirstOrDefault(u => u.Login == login);
        }

        public List<UsuarioModel> BuscarTodos()
        {
            return _dataContext.Usuarios
                .Include(x => x.Contatos)
                .ToList();
        }

        public UsuarioModel ListarPorId(int id)
        {
            return _dataContext.Usuarios.FirstOrDefault(u => u.Id == id);
        }
    }
}
