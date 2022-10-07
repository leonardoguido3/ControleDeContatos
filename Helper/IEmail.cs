namespace ControleDeContatos.Helper
{
    public interface IEmail
    {
        //Interface com o metodo Enviar, para assinarmos em nossos metodos
        bool Enviar(string email, string assunto, string mensagem);
    }
}
