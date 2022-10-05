using ControleDeContatos.Data;
using ControleDeContatos.Models;

namespace ControleDeContatos.Repositorio
{
    public class ContatoRepositorio : IContatoRepositorio
    {
        private readonly DataContext _dataContext;
        public ContatoRepositorio(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<ContatoModel> BuscarTodos()
        {
            return _dataContext.Contatos.ToList();
        }

        public ContatoModel ListarPorId(int id)
        {
            return _dataContext.Contatos.FirstOrDefault(c => c.Id == id);
        }

        public ContatoModel Adicionar(ContatoModel contato)
        {
            // Aqui terá regras de negocio para gravar no banco de dados
            _dataContext.Contatos.Add(contato);
            _dataContext.SaveChanges();
            return contato;
        }

        public ContatoModel Atualizar(ContatoModel contato)
        {
            ContatoModel contatoDB = ListarPorId(contato.Id);
            if (contatoDB == null) throw new Exception("Houve um erro na atualização deste contato!");
            contatoDB.Bairro = contato.Bairro;
            contatoDB.Cidade = contato.Cidade;
            contatoDB.Telefone = contato.Telefone;
            contatoDB.Tipo = contato.Tipo;
            contatoDB.Observacao = contato.Observacao;

            _dataContext.Contatos.Update(contatoDB);
            _dataContext.SaveChanges();

            return contatoDB;

        }

        public bool Apagar(int id)
        {
            ContatoModel contatoDB = ListarPorId(id);
            if (contatoDB == null) throw new Exception("Houve um erro na exclusão do contato!");
            _dataContext.Remove(contatoDB);
            _dataContext.SaveChanges();

            return true;
        }

        public List<ContatoModel> BuscarTodosIdUsuario(int usuarioId)
        {
            return _dataContext.Contatos.Where(x => x.UsuarioId == usuarioId).ToList();
        }
    }
}
