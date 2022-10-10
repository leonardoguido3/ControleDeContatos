namespace ControleDeContatos.Models
{
    public class CidadeModel
    {
        //Modelo do banco de dados para inserção de cidades no Select recebendo uma lista de contatos cadastrados

        public int Id { get; set; }
        public string Cidade { get; set; }
        public virtual List<ContatoModel> Contatos { get; set; }
    }
}
