using ControleDeContatos.Filters;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    [PaginaRestritaSomenteAdmin]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        public UsuarioController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }
        public IActionResult Index()
        {
            List<UsuarioModel> usuarios = _usuarioRepositorio.BuscarTodos();
            return View(usuarios);
        }

        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(UsuarioModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _usuarioRepositorio.Adicionar(usuario);
                    TempData["MensagemSucesso"] = "Usuário cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(usuario);
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não conseguimos realizar o cadastro, tente novamente!";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Editar(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }

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

        public IActionResult ApagarConfirmacao(int id)
        {
            UsuarioModel usuario = _usuarioRepositorio.ListarPorId(id);
            return View(usuario);
        }

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
