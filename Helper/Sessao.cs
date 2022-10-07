using ControleDeContatos.Models;
using Newtonsoft.Json;

namespace ControleDeContatos.Helper
{
    public class Sessao : ISessao
    {
        //Injeção da IHttpContextAccessor
        private readonly IHttpContextAccessor _contextAccessor;

        //Construtor da IHTTP
        public Sessao(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        //Metodo para buscar uma sessão ativa, criamos uma variavel e temos tratativas, caso tenha conteudo, vamos deserializar o JSON com as informações obtidas
        public UsuarioModel BuscarSessaoDoUsuario()
        {
            string sessaoUsuario = _contextAccessor.HttpContext.Session.GetString("sessaoUsuarioLogado");

            if (string.IsNullOrEmpty(sessaoUsuario)) return null;
            return JsonConvert.DeserializeObject<UsuarioModel>(sessaoUsuario);
        }

        //Metodo para Criar uma sessão, é serializado para um objeto JSON os dados de usuario, e enviado para a chave de sessão logada com o valor serializado
        public void CriarSessaoDoUsuario(UsuarioModel usuario)
        {
            string valor = JsonConvert.SerializeObject(usuario);
            _contextAccessor.HttpContext.Session.SetString("sessaoUsuarioLogado", valor);
        }

        //Metodo de Remover sessão, aqui chamamos o remove e passamos a chave de sseão
        public void RemoverSessaoUsuario()
        {
            _contextAccessor.HttpContext.Session.Remove("sessaoUsuarioLogado");
        }
    }
}
