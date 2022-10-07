using ControleDeContatos.Filters;
using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    //Usuario precisa estar logado para ter acesso a esta funcionalidade
    [PaginaParaUsuarioLogado]

    public class AlterarSenhaController : Controller
    {
        //Injeção de dependencias do Repositorio Usuario e do Sessao
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;

        //Construtor do Usuario e Sessao
        public AlterarSenhaController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }

        //Função padrão chamando a página index para alteração de senha
        public IActionResult Index()
        {
            return View();
        }

        //Metodo POST pegando a model AlterarSenha e realizando tratativas para a alteração
        [HttpPost]
        public IActionResult Alterar(AlterarSenhaModel alterarSenhaModel)
        {
            try
            {
                UsuarioModel usuarioLogado = _sessao.BuscarSessaoDoUsuario();
                alterarSenhaModel.Id = usuarioLogado.Id;

                if (ModelState.IsValid)
                {
                    _usuarioRepositorio.AlterarSenha(alterarSenhaModel);
                    TempData["MensagemSucesso"] = "Senha alterada com sucesso!";
                    return View("Index", alterarSenhaModel);
                }
                return View("Index", alterarSenhaModel);

            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não foi possível alterar sua senha, tente novamente!";
                return View("Index", alterarSenhaModel);
            }
        }
    }
}
