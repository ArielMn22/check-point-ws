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
            email.UsuarioEmail = form["userEmail"];
            email.Mensagem = $"E-mail do usuário: {email.UsuarioEmail}\nMensagem: {form["emailMensagem"]}";
            email.Destino = "arielpaixao10@gmail.com";

            Execute().Wait();

            return RedirectToAction("Contact", "Pages");
        }

        [HttpPost]
        public static void EnviarCadastro(UsuarioModel usuario)
        {
            email.Mensagem = "Seu usuário foi cadastrado com sucesso! Você já pode logar em nosso sistema a qualquer momento e deixar seu comentário!";
            email.UsuarioNome = usuario.Nome;
            email.UsuarioEmail = usuario.Email;
            email.Destino = usuario.Email;

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