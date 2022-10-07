using ControleDeContatos.Data;
using ControleDeContatos.Models;

namespace ControleDeContatos.Repositorio
{
    public class ContatoRepositorio : IContatoRepositorio
    {
        //Injetor de dependencia DataContext
        private readonly DataContext _dataContext;

        //Construtor
        public ContatoRepositorio(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        //Função para buscar todos os contatos, criando uma lista
        public List<ContatoModel> BuscarTodos()
        {
            return _dataContext.Contatos.ToList();
        }

        //Função de listar por ID, pegando o primeiro ou o unico, comparando o ID passado com o ID do objeto a ser listado
        public ContatoModel ListarPorId(int id)
        {
            return _dataContext.Contatos.FirstOrDefault(c => c.Id == id);
        }

        //Função para adicionar um contato no banco de dados, retornando o proprio contato para a controller
        public ContatoModel Adicionar(ContatoModel contato)
        {
            
            _dataContext.Contatos.Add(contato);
            _dataContext.SaveChanges();
            return contato;
        }

        //Função para alterar um contato, recebendo o contato a ser alterado, antes é realizado uma tratativa, para que o contato não venha nulo
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

        //Função para apagar um contato, recebendo o contato a ser deletado, antes é realizado uma tratativa, para que o contato a ser deletado seja nulo
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
