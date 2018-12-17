using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CheckPoint.Sistema.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CheckPoint.Sistema.Controllers {
    public class EmailController : Controller {

        static EmailModel email = new EmailModel();

        [HttpPost]
        public IActionResult EnviarContato(IFormCollection form)
        {
            email.UsuarioNome = form["usuarioNome"];
            email.Destino = "arielpaixao10@gmail.com";
            email.Mensagem = $"E-mail do usuário: {email.Destino}\nMensagem: {form["emailMensagem"]}";

            Execute().Wait();

            return RedirectToAction("Contact", "Pages");
        }

        [HttpPost]
        public static void EnviarCadastro(UsuarioModel usuario)
        {
            email.Mensagem = "Seu usuário foi cadastrado com sucesso! Você já pode logar em nosso sistema a qualquer momento e deixar seu comentário!";
            email.UsuarioNome = usuario.Nome;
            email.Destino = usuario.Email;

            Execute().Wait();
        }

        [HttpPost]
        public static void EnviarAvaliacaoAprovada(ComentarioModel comentario)
        {
            email.Mensagem = $"Seu comentario foi aprovado com sucesso! Agora você já pode vê-lo em nosso Home Page! **Seu comentário: {comentario.Mensagem}**";
            email.UsuarioNome = comentario.Usuario.Nome;
            email.Destino = comentario.Usuario.Email;

            Execute().Wait();
        }

        static async Task Execute()
        {
            var apiKey = "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";

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("checkpoint.arielmn22@gmail.com", "Check Point");

            var subject = $"Contato - {email.UsuarioNome}";

            var to = new EmailAddress(email.Destino);

            var plainTextContent = email.Mensagem;

            var htmlContent = email.Mensagem;

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
        }
    }
}