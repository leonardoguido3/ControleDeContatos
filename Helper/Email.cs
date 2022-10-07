using System.Net;
using System.Net.Mail;

namespace ControleDeContatos.Helper
{
    public class Email : IEmail
    {
        //Injeção da IConfiguration para envio de email
        private readonly IConfiguration _configuration;

        //Construtor do objeto
        public Email(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Metodo booleano, que recebe como parametro email, assunto, mensagem, capturamos da nossa AppSettingsJSON as informações do servidor de email, chamamos o MailMessage e construimos o metodo de envio
        public bool Enviar(string email, string assunto, string mensagem)
        {
            try
            {
                string host = _configuration.GetValue<string>("SMTP:Host");
                string nome = _configuration.GetValue<string>("SMTP:Nome");
                string username = _configuration.GetValue<string>("SMTP:UserName");
                string senha = _configuration.GetValue<string>("SMTP:Senha");
                int porta = _configuration.GetValue<int>("SMTP:Porta");

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(username, nome)
                };

                mail.To.Add(email);
                mail.Subject = assunto;
                mail.Body = mensagem;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(host, porta))
                {
                    smtp.Credentials = new NetworkCredential(username, senha);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    return true;
                }


            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
