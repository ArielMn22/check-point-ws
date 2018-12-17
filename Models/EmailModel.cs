namespace CheckPoint.Sistema.Models
{
    public class EmailModel
    {
        /// <summary>
        /// Nome do usuário para o qual será enviado o e-mail.
        /// </summary>
        public string UsuarioNome { get; set; }
        /// <summary>
        /// Endereço de e-mail para o qual a mensagem será enviada.
        /// </summary>
        public string Destino { get; set; }
        /// <summary>
        /// Mensagem a ser enviada dentro do e-mail.
        /// </summary>
        public string Mensagem { get; set; }
    }
}