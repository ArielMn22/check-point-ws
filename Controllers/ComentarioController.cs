using System;
using check_point.sistema.Sessions;
using CheckPoint.Sistema.Enums;
using CheckPoint.Sistema.Interfaces;
using CheckPoint.Sistema.Models;
using CheckPoint.Sistema.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckPoint.Sistema.Controllers {
    public class ComentarioController : Controller {
        private IComentario ComentarioRepositorio { get; set; }
        public ComentarioController () {
            ComentarioRepositorio = new ComentarioRepositorio ();
        }

        [HttpPost]
        public IActionResult Cadastrar (IFormCollection form) {

            if (!string.IsNullOrEmpty (form["comentario"])) {
                UsuarioModel usuario = HttpContext.Session.GetObject<UsuarioModel> ("usuarioLogado");

                string statusComentario;

                if (usuario.Administrador) {
                    statusComentario = TiposComentario.Aceito.ToString ();
                } else {
                    statusComentario = TiposComentario.EmEspera.ToString ();
                }

                ComentarioModel comentarioModel = new ComentarioModel (
                    //obter e armazenar o usuario da session
                    usuario : usuario,
                    msg : form["comentario"],
                    data : DateTime.Now,
                    status : statusComentario,
                    nota : int.Parse (form["avaliacao"])
                );

                ComentarioRepositorio.Cadastrar (comentarioModel);
                TempData["Comentario"] = "Comentário efetuado com sucesso!";
            } else {
                TempData["ComentarioErro"] = "Você não pode enviar um comentário vazio.";
            }

            return RedirectToAction ("Home", "Pages");
        }

        static string tipoGerenciar;

        [HttpGet]
        public IActionResult Gerenciar () {

            if (HttpContext.Session.GetObject<UsuarioModel> ("usuarioLogado") != null) {

                UsuarioModel usuario = HttpContext.Session.GetObject<UsuarioModel> ("usuarioLogado");

                if (usuario.Administrador) {

                    ComentarioRepositorio comentarioRep = new ComentarioRepositorio ();

                    /// <summary>
                    /// Retorna somente os comentários com status igual ao passado pelo parâmetro.
                    /// </summary>
                    /// <returns></returns>

                    if (tipoGerenciar == "aceito") {
                        ViewData["Comentarios"] = comentarioRep.ListarComentariosEspecifico (TiposComentario.Aceito.ToString ());
                        ViewBag.TipoComentario = "Comentários Aceitos";
                    } else if (tipoGerenciar == "recusado") {
                        ViewData["Comentarios"] = comentarioRep.ListarComentariosEspecifico (TiposComentario.Recusado.ToString ());
                        ViewBag.TipoComentario = "Comentários Recusados";
                    } else {
                        ViewData["Comentarios"] = comentarioRep.ListarComentariosEspecifico (TiposComentario.EmEspera.ToString ());
                        ViewBag.TipoComentario = "Comentários em Espera";
                    }

                    return View ();
                } else {
                    return RedirectToAction ("Home", "Pages");
                }
            } else {
                return RedirectToAction ("Login", "Usuario");
            }

        }

        [HttpPost]
        public IActionResult Gerenciar (IFormCollection form) {
            int CommentId = int.Parse (form["commentId"]);

            ComentarioRepositorio comentarioRep = new ComentarioRepositorio ();

            ComentarioModel comentarioModel = comentarioRep.BuscarPorId (CommentId);

            if (form["choice"] == "aceito") {
                comentarioRep.Editar (TiposComentario.Aceito.ToString (), comentarioModel);
            } else {
                if (form["choice"] == "recusado") {
                    comentarioRep.Editar (TiposComentario.Recusado.ToString (), comentarioModel);
                } else {
                    ViewBag.Mensagem = "Opção inválida!";
                    return View ();
                }
            }

            ViewBag.Mensagem = $"Status do comentário de Id '{comentarioModel.Id}' foi 💕🐱‍💻😉🌹🐱‍🐉🐱‍👓🐱‍👓🤔🤔🍣!";

            return RedirectToAction ("Gerenciar");
        }

        [HttpGet]
        public IActionResult Todos () {

            if (HttpContext.Session.GetObject<UsuarioModel> ("usuarioLogado") != null) {
                UsuarioModel usuario = HttpContext.Session.GetObject<UsuarioModel> ("usuarioLogado");

                string[] nomes = usuario.Nome.Split (" ");

                ViewBag.UsuarioLogado = nomes[0];
            } else {
                ViewBag.UsuarioLogado = null;
            }

            ComentarioRepositorio comentarioRep = new ComentarioRepositorio ();

            /// <summary>
            /// Retorna somente os comentários com status igual ao passado pelo parâmetro.
            /// </summary>
            /// <returns></returns>
            ViewData["ComentariosAceitos"] = comentarioRep.ListarComentariosEspecifico (TiposComentario.Aceito.ToString ());

            return View ();
        }

        [HttpPost]
        public IActionResult TipoGerenciar (IFormCollection form) {
            if (form["tipoGerenciar"] == "aceito") {
                tipoGerenciar = "aceito";
            } else if (form["tipoGerenciar"] == "recusado") {
                tipoGerenciar = "recusado";
            } else {
                tipoGerenciar = "emEspera";
            }

            return RedirectToAction ("Gerenciar");
        }
    }
}