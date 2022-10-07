using ControleDeContatos.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ControleDeContatos.ViewComponents
{
    //Herdando a view components para controlar os componentes a ser visualizado pelo usuarios
    public class Menu : ViewComponent
    {
        //Metodo async onde captura a sessao do usuario e deserializa o JSON retornando um objeto, realizado antes tratativas caso venha nulo a sessao para exibição do menu de acordo com o perfil de acesso
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string sessaoUsuario = HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario)) return null;

            UsuarioModel usuario = JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);

            return View(usuario);
        }
    }
}
