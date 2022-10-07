using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    //Usuario precisa estar logado e ser administrador do sistema para ter acesso a esta funcionalidade
    [PaginaRestritaSomenteAdmin]
    public class UsuarioController : Controller
    {
        //Injeção das dependencias Repositorio Usuario e Contato
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IContatoRepositorio _contatoRepositorio;

        //Construtor do Usuario e Contato
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio, IContatoRepositorio contatoRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _contatoRepositorio = contatoRepositorio;
        }

        //Função padrão chamando a página index para a pagina de usuario, buscando todos cadastrados
        public IActionResult Index()
        {
            List<UsuarioModel> usuarios = _usuarioRepositorio.BuscarTodos();
            return View(usuarios);
        }

        //Funcao padrao chamando a pagina de criação de usuario
        public IActionResult Criar()
        {
            return View();
        }

        //Funcao para listar todos os contatos cadastrados por ID do usuario, é filtrado e adicionado a cada usuario a quantidade de contatos cadastrados
        public IActionResult ListarContatosPorUsuarioId(int id)
        {
            List<ContatoModel> contatos = _contatoRepositorio.BuscarTodosIdUsuario(id);
            return PartialView("_ContatosUsuario", contatos);
        }

        //Função POST de criação de usuario, pegamos todos os dados, realizamos uma tratativa, se tudo estiver correto, é enviado para cadastro
        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            _usuarioRepositorio.Adicionar(usuario);
            TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
            return RedirectToAction("Index");
            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        _usuarioRepositorio.Adicionar(usuario);
            //        TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
            //        return RedirectToAction("Index");
            //    }
            //    return View(usuario);
            //}
            //catch (Exception)
            //{
            //    TempData["MensagemErro"] = "Ops, não conseguimos realizar o cadastro, tente novamente!";
            //    return RedirectToAction("Index");
            //}
        }

        //Funcao edição de usuario, passamos o ID do usuario para a função de edição
        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }

        //Função POST para alteração de usuario propriamente dita. Aqui pegamos os dados, fazemos uma tratativa e chamamos o metodo de alteração
        [HttpPost]
        public IActionResult Alterar(UsuarioModel usuario)
        {
            _usuarioRepositorio.Atualizar(usuario);
            TempData["MensagemSucesso"] = "Usuário alterado com sucesso!";
            return RedirectToAction("Index");

            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        _usuarioRepositorio.Atualizar(usuario);
            //        TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
            //        return RedirectToAction("Index");
            //    }
            //    return View("Editar", usuario);

            //}
            //catch (Exception)
            //{
            //    TempData["MensagemErro"] = "Ops, não conseguimos alterar o usuario, tente novamente!";
            //    return RedirectToAction("Index");
            //}

        }

        //Metodo que confirma a deleção do usuario, não é a deleção em si, apenas confirma e pega o ID do usuario que sera deletado
        public IActionResult ApagarConfirmacao(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }

        //Metodo para deletar o usuario propriamente dito, fazemos uma tratativa para verificar se tudo está correto, pegamos o ID capturado e é realizado a remoção
        public IActionResult Apagar(int id)
        {
            try
            {
                bool apagado = _usuarioRepositorio.Apagar(id);
                if (apagado)
                {
                    TempData["MensagemSucesso"] = "Usuário apagado com sucesso!";
                }
                else
                {
                    TempData["MensagemErro"] = "Ops, não foi possível apagar o usuário, tente novamente!";
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não foi possível apagar o usuário, tente novamente!";
                return RedirectToAction("Index");
            }

        }
    }
}
