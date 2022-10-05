using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        private readonly IEmail _email;
        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao, IEmail email)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _email = email;
        }
        public IActionResult Index()
        {
            // Se o usuario estiver logado, redirecionar para HOME
            if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);
                    
                    if (usuario != null)
                    {
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            _sessao.CriarSessaoDoUsuario(usuario);
                            return RedirectToAction("Index", "Home");
                        }
                        TempData["MensagemErro"] = "Ops, senha do usuário é inválida, tente novamente!";
                    }
                    TempData["MensagemErro"] = "Ops, usuário e/ou senha inválidos, tente novamente!";
                }
                return View("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não conseguimos realizar o login, tente novamente!";
                return RedirectToAction("Index");
            }
        }

        public IActionResult RedefinirSenha()
        {
            return View();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public IActionResult EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = _usuarioRepositorio.BuscarPorEmailELogin(redefinirSenhaModel.Email, redefinirSenhaModel.Login);

                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();                       
                        string mensagem = $"Olá, este é um email de recuperação de senha. Sua nova senha é: {novaSenha}";
                        bool emailEnviado = _email.Enviar(usuario.Email, "CRM - Max Net | Nova Senha", mensagem);

                        if (emailEnviado)
                        {
                            _usuarioRepositorio.Atualizar(usuario);
                            TempData["MensagemSucesso"] = $"Enviamos a nova senha para: {redefinirSenhaModel.Email}!";
                        }
                        else
                        {
                            TempData["MensagemErro"] = "Ops, não foi possivel enviar o email, tente mais tarde!";
                        }
                        
                        return RedirectToAction("Index", "Login");
                    }
                    TempData["MensagemErro"] = "Ops, não foi possivel redefinir sua senha, verifique os dados informados!";
                }
                return View("Index");
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = "Ops, não conseguimos redefinir sua senha, tente novamente!";
                return RedirectToAction("Index");
            }
        }

    }
}
