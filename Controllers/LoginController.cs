using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    public class LoginController : Controller
    {
        //Injeção de dependencias do Repositorio Usuario, Sessao e Email
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;
        private readonly IEmail _email;

        //Construtor do Usuario, Sessao e Email
        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao, IEmail email)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _email = email;
        }
        //Metodo index padrão, chamando a pagina prinicipal, mas antes realizando a verificação se o usuario tá logado
        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");
            return View();
        }

        //Metodo de logar no sistema, realizando as tratativas e gerando a sessao do usuario
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

        //Metodo index redefinir senha
        public IActionResult RedefinirSenha()
        {
            return View();
        }

        //Metodo para sair, removendo assim a sessao do usuario e enviando ele para tela de login
        public IActionResult Sair()
        {
            _sessao.RemoverSessaoUsuario();
            return RedirectToAction("Index", "Login");
        }

        //Metodo POST para envio de email, onde é realizado as tratativas dos valores, e se tudo estiver correto, é enviado um email com a nova senha
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
