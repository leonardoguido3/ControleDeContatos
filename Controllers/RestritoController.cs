using ControleDeContatos.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    //Usuario precisa estar logado para ter acesso a esta funcionalidade
    [PaginaParaUsuarioLogado]
    public class RestritoController : Controller
    {
        //Metodo para chamar a index da página restrita quando usuario tentar acessar algum conteudo sem ter uma sessao ativa, ou tentar entrar em funções administrativas sendo usuario comum
        public IActionResult Index()
        {
            return View();
        }
    }
}
