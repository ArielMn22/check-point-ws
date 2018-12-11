using System;
using CheckPoint.Sistema.Interfaces;
using CheckPoint.Sistema.Models;
using CheckPoint.Sistema.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using check_point.sistema.Sessions;

namespace CheckPoint.Sistema.Controllers {
    public class UsuarioController : Controller {
       
        public IUsuario UsuarioRepositorio { get; set; }

        public UsuarioController () {
            UsuarioRepositorio = new UsuarioRepositorio ();
        }

        [HttpGet]
        public IActionResult Cadastrar () {

            if (HttpContext.Session.GetObject<UsuarioModel>("usuarioLogado") != null) {
                UsuarioModel usuario = HttpContext.Session.GetObject<UsuarioModel>("usuarioLogado");

                string[] nomes = usuario.Nome.Split (" ");

                ViewBag.UsuarioLogado = nomes[0];
            } else {
                ViewBag.UsuarioLogado = null;
            }

            return View ();
        }

        [HttpPost]
        public IActionResult Cadastrar (IFormCollection form) {

            switch (UsuarioRepositorio.ValidaUsuario (form)) {
                case 0:
                    bool admin;

                    if (form["tipo"] == "true") {
                        admin = true;
                    } else {
                        admin = false;
                    }

                    UsuarioModel usuario = new UsuarioModel (
                        nome: form["nome"],
                        email: form["email"],
                        senha: form["senha"],
                        admin: admin
                    );

                    UsuarioRepositorio.Cadastrar (usuario);

                    EmailController.EnviarCadastro (usuario);

                    TempData["MensagemLogin"] = "Usuario cadastrado com sucesso! Você já pode logar em nosso sistema!";

                    return RedirectToAction ("Login");

                case 1:
                    TempData["MensagemCadastroUser"] = "O campo nome não pode estar vazio.";
                    break;

                case 2:
                    TempData["MensagemCadastroUser"] = "O e-mail informado já foi cadastrado em nosso sistema.";
                    break;

                case 3:
                    TempData["MensagemCadastroUser"] = "As senhas informadas não coincidem.";
                    break;

                case 4:
                    TempData["MensagemCadastroUser"] = "A senha informada deve possuir pelo menos 6 caractéres.";
                    break;

                default:
                    TempData["MensagemCadastroUser"] = "Desculpe, houve um erro, tente novamente mais tarde.";
                    break;
            }

            return View ();
        }

        [HttpGet]
        public IActionResult Login () {

            if (HttpContext.Session.GetObject<UsuarioModel>("usuarioLogado") != null) {
                TempData["MensagemLogin"] = "Saia da sessão atual para realizar outro login.";

                return RedirectToAction ("Cadastrar");

            } else {
                ViewBag.UsuarioLogado = null;
            }

            return View ();
        }

        [HttpPost]
        public IActionResult Login (IFormCollection form) {
            UsuarioModel usuario = UsuarioRepositorio.Login (form["email"], form["senha"]);

            if (usuario != null) {
                // Coloca o objeto 'usuario' dentro da session.
                HttpContext.Session.SetObject("usuarioLogado", usuario);

                return RedirectToAction ("Home", "Pages");
            } else {
                ViewBag.Mensagem = "E-mail ou senha incorretos.";
                return View ();
            }
        }

        [HttpGet]
        public IActionResult Deslogar () {
            HttpContext.Session.Clear ();

            return RedirectToAction ("Home", "Pages");
        }

    }
}