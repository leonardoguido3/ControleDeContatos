using ControleDeContatos.Enums;
using ControleDeContatos.Helper;
using System.ComponentModel.DataAnnotations;

namespace ControleDeContatos.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o nome do usuário!")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Digite o login do usuário!")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Digite o email do usuário!")]
        [EmailAddress(ErrorMessage = "O email informado não é válido!")]
        public string Email { get; set; }
        public PerfilEnum Perfil { get; set; }
        [Required(ErrorMessage = "Digite a senha do usuário!")]
        public string Senha { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public virtual List<ContatoModel> Contatos { get; set; }


        //Responsabilidade de validar senha
        public bool SenhaValida(string senha)
        {
            return Senha == senha.GerarHash();
        }

        //Responsabilidade de gerar o HASH
        public void SetSenhaHash()
        {
            Senha = Senha.GerarHash();
        }

        //Responsabilidade de gerar nova senha (recuperacao)
        public string GerarNovaSenha()
        {
            string novaSenha = Guid.NewGuid().ToString().Substring(0, 8);
            Senha = novaSenha.GerarHash();
            return novaSenha;
        }

        //Responsabilidade de alterar a senha do usuario
        public void SetNovaSenha(string novaSenha)
        {
            Senha = novaSenha.GerarHash();
        }

    }

}
