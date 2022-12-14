using System.ComponentModel.DataAnnotations;

namespace ControleDeContatos.Models
{
    public class ContatoModel
    {
        //Modelo do banco de dados de contatos, com o objeto vindo do relacionamento com o usuario
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o nome do contato!")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Digite o bairro do contato!")]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "Digite o telefone do contato!")]
        [Phone(ErrorMessage = "O numero informado não é válido!")]
        public string Telefone { get; set; }
        public string? Tipo { get; set; }
        public string? Observacao { get; set; }
        public int? UsuarioId { get; set; }
        public UsuarioModel Usuario { get; set; }
        public int? CidadeId { get; set; }
        [Required(ErrorMessage = "Digite a cidade do contato!")]
        public CidadeModel Cidade { get; set; }
    }
}
