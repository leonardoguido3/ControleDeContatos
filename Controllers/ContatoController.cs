using ControleDeContatos.Filters;
using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ControleDeContatos.Controllers
{
    //Usuario precisa estar logado para ter acesso a esta funcionalidade
    [PaginaParaUsuarioLogado]

    public class ContatoController : Controller
    {
        //Injeção de dependencias do Repositorio Contato e do Sessao
        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly ISessao _sessao;

        //Construtor do Contato e Sessao
        public ContatoController(IContatoRepositorio contatoRepositorio, ISessao sessao)
        {
            _contatoRepositorio = contatoRepositorio;
            _sessao = sessao;
        }

        //Metodo index padrão, chamando a pagina prinicipal, mas antes realizando a verificação de Perfil do usuario para injetar o conteudo dentro da view de acordo com o ID
        public IActionResult Index()
        {
            UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            if (usuarioLogado.Perfil == Enums.PerfilEnum.Admin)
            {
                List<ContatoModel> contatosTotal = _contatoRepositorio.BuscarTodos();
                return View(contatosTotal);
            }
            List<ContatoModel> contatos = _contatoRepositorio.BuscarTodosIdUsuario(usuarioLogado.Id);
            return View(contatos);
        }

        //Metodo padrão chamando a pagina de criar novo contato
        public IActionResult Criar()
        {
            return View();
        }

        //Metodo de edição, recebendo ID do objeto a ser alterado
        public IActionResult Editar(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }

        //Metodo de confirmação, este metodo não apaga, apenas confirma se é realmente de desejo do usuario. Se sim, é enviado para a função de apagar realmente
        public IActionResult ApagarConfirmacao(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }

        //Metodo POST de criação, aqui é realizado a tratativa e validações chamando o metodo para inserir os dados na devida tabela, enviando também os dados do usuario que solicitou
        [HttpPost]
        public IActionResult Criar(ContatoModel contato)
        {
            try
            {
                UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                contato.UsuarioId = usuarioLogado.Id;
                _contatoRepositorio.Adicionar(contato);
                TempData["MensagemSucesso"] = "Contato cadastrado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não conseguimos realizar o cadastro, tente novamente!";
                return RedirectToAction("Index");
            }
        }

        //Metodo POST de alteração, aqui é realizado a tratativa e validações chamando o metodo para inserir os dados devido na tabela para alteração, enviando também os dados do usuario que solicitou
        [HttpPost]
        public IActionResult Alterar(ContatoModel contato)
        {
            try
            {
                UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                contato.UsuarioId = usuarioLogado.Id;
                _contatoRepositorio.Atualizar(contato);
                TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não conseguimos alterar o contato, tente novamente!";
                return RedirectToAction("Index");
            }

        }

        //Metodo de exclusão real do contato, aqui é realizado as tratativas, que se validos realmente exclui o contato
        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _contatoRepositorio.Apagar(id);
                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Contato apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não foi possível apagar o contato, tente novamente!";
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não foi possível apagar o contato, tente novamente!";
                return RedirectToAction("Index");
            }

        }
    }
}
