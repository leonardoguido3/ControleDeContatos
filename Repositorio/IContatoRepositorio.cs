using ControleDeContatos.Models;

namespace ControleDeContatos.Repositorio
{
    public interface IContatoRepositorio
    {
        //Lista de contratos a serem assinados pelo repositorio para realizar funções do CRUD Contato
        List<ContatoModel> BuscarTodos();
        List<ContatoModel> BuscarTodosIdUsuario(int usuarioId);
        ContatoModel ListarPorId(int id);
        ContatoModel Adicionar(ContatoModel contato);
        ContatoModel Atualizar(ContatoModel contato);
        bool Apagar(int id);
    }
}
