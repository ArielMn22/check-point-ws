using System.Collections.Generic;
using check_point.sistema.Sessions;
using CheckPoint.Sistema.Enums;
using CheckPoint.Sistema.Models;
using CheckPoint.Sistema.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckPoint.Sistema.Controllers {
    public class PagesController : Controller {
        [HttpGet]
        public IActionResult Home () {
    
            if (HttpContext.Session.GetObject<UsuarioModel>("usuarioLogado") != null) {
                UsuarioModel usuario = HttpContext.Session.GetObject<UsuarioModel>("usuarioLogado");

                string[] nomes = usuario.Nome.Split (" ");

                ViewBag.UsuarioLogado = nomes[0];
            } else {
                ViewBag.UsuarioLogado = null;
            }

            ComentarioRepositorio comentario = new ComentarioRepositorio();

            List<ComentarioModel> comentariosAceitos = comentario.ListarComentariosEspecifico(TiposComentario.Aceito.ToString());
            
            ViewData["ComentariosAceitos"] = comentariosAceitos.Count <= 10 ? comentariosAceitos : comentariosAceitos.GetRange(0,10);

            return View ();
        }

        [HttpGet]
        public IActionResult About () {
 
            if (HttpContext.Session.GetObject<UsuarioModel>("usuarioLogado") != null) {
                UsuarioModel usuario = HttpContext.Session.GetObject<UsuarioModel>("usuarioLogado");

                string[] nomes = usuario.Nome.Split (" ");

                ViewBag.UsuarioLogado = nomes[0];
            } else {
                ViewBag.UsuarioLogado = null;
            }
            return View ();
        }

        [HttpGet]
        public IActionResult Faq () {

            if (HttpContext.Session.GetObject<UsuarioModel>("usuarioLogado") != null) {
                UsuarioModel usuario = HttpContext.Session.GetObject<UsuarioModel>("usuarioLogado");

                string[] nomes = usuario.Nome.Split (" ");

                ViewBag.UsuarioLogado = nomes[0];
            } else {
                ViewBag.UsuarioLogado = null;
            }
            return View ();
        }

        [HttpGet]
        public IActionResult Contact () {

            if (HttpContext.Session.GetObject<UsuarioModel>("usuarioLogado") != null) {
                UsuarioModel usuario = HttpContext.Session.GetObject<UsuarioModel>("usuarioLogado");

                string[] nomes = usuario.Nome.Split (" ");

                ViewBag.UsuarioLogado = nomes[0];
            } else {
                ViewBag.UsuarioLogado = null;
            }

            return View ();
        }
    }
}