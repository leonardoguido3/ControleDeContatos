using System.ComponentModel.DataAnnotations;

namespace ControleDeContatos.Models
{
    public class RedefinirSenhaModel
    {
        //Modelo que não é salvo no banco, é pego pelo input do cliente, e realizado pelo back a comparação
        [Required(ErrorMessage = "Digite o login!")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Digite o email!")]
        public string Email { get; set; }
    }
}
