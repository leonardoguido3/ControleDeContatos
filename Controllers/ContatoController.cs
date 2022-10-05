using ControleDeContatos.Filters;
using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    [PaginaParaUsuarioLogado]
    public class ContatoController : Controller
    {
        private readonly IContatoRepositorio _contatoRepositorio;
        private readonly ISessao _sessao;

        public ContatoController(IContatoRepositorio contatoRepositorio, ISessao sessao)
        {
            _contatoRepositorio = contatoRepositorio;
            _sessao = sessao;
        }
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

        public IActionResult Criar()
        {
            return View();
        }

        public IActionResult Editar(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }

        public IActionResult ApagarConfirmacao(int id)
        {
            ContatoModel contato = _contatoRepositorio.ListarPorId(id);
            return View(contato);
        }

        [HttpPost]
        public IActionResult Criar(ContatoModel contato)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                    contato.UsuarioId = usuarioLogado.Id;
                    _contatoRepositorio.Adicionar(contato);
                    TempData["MensagemSucesso"] = "Contato cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(contato);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não conseguimos realizar o cadastro, tente novamente!";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Alterar(ContatoModel contato)
        {
            UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
            contato.UsuarioId = usuarioLogado.Id;
            _contatoRepositorio.Atualizar(contato);
            TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
            return RedirectToAction("Index");

            //try
            //{
            //    if (ModelState.IsValid)
            //    {
            //        _contatoRepositorio.Atualizar(contato);
            //        TempData["MensagemSucesso"] = "Contato alterado com sucesso!";
            //        return RedirectToAction("Index");
            //    }
            //    return View("Editar", contato);

            //}
            //catch (Exception)
            //{
            //    TempData["MensagemErro"] = "Ops, não conseguimos alterar o contato, tente novamente!";
            //    return RedirectToAction("Index");
            //}

        }

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
