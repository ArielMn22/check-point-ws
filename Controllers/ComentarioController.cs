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

        /// <summary>
        /// Declara a a interface IComentario, para que o 'ComentarioRepositorio' seja Polimorfado.
        /// </summary>
        /// <value>Será instanciado como um ComentarioRepositório, de que tipo? Quem sabe? "POLIMORFISMO"!</value>
        private IComentario ComentarioRepositorio { get; set; }

        /// <summary>
        /// Método construtor que gera a interface ComentarioRepositorio instanciada com 'new ComentarioRepositorio'.
        /// </summary>
        public ComentarioController () {
            ComentarioRepositorio = new ComentarioRepositorio ();
        }

        /// <summary>
        /// Contrói um ComentarioModel e o envia para o Cadastro no Repositório.
        /// </summary>
        /// <param name="form">Dados da View().</param>
        /// <returns>Retorna uma página para o usuário.</returns>
        [HttpPost]
        public IActionResult Cadastrar (IFormCollection form) {

            // Se o campo de comentário não estiver vazio, inicie o processo de cadastro de comentário.
            if (!string.IsNullOrEmpty (form["comentario"])) {
                // Retorna um UsuarioModel da Session. Usuário este que está logado.
                UsuarioModel usuario = HttpContext.Session.GetObject<UsuarioModel> ("usuarioLogado");

                // Declara uma string que será utilizada para definir qual será o Status do comentário.
                string statusComentario;

                // Se o usuário que estiver logado for administrador...
                if (usuario.Administrador) {
                    // O status do comentário será aceito automaticamente.
                    statusComentario = TiposComentario.Aceito.ToString ();
                } else {
                    // O status do comentário será em espera automaticamente.
                    // Para ser aceito o comentário deverá ser aceito na Dashboard de um usuário administrador.
                    statusComentario = TiposComentario.EmEspera.ToString ();
                }

                // Declara e monta um ComentarioModel por meio do método contrutor.
                ComentarioModel comentarioModel = new ComentarioModel (
                    // Propriedade que guarda o usuário que realizou o comentário.
                    usuario : usuario,
                    // Guarda a mensagem do comentário.
                    msg : form["comentario"],
                    // Guarda a data em que o comentário foi realizado.
                    data : DateTime.Now,
                    // Guarda o Status do comentário.
                    status : statusComentario,
                    // Guarda a avaliação do usuário.
                    nota : int.Parse (form["avaliacao"])
                );

                // Chama o método Cadastrar do repositório polimorfado para que ele grave no arquivo;
                ComentarioRepositorio.Cadastrar (comentarioModel);

                // Retorna uma mensagem para a View() do usuário.
                TempData["Comentario"] = "Comentário efetuado com sucesso!";
            } else {
                // Retorna uma mensagem para a View() do usuário.
                TempData["ComentarioErro"] = "Você não pode enviar um comentário vazio.";
            }

            // Retorna uma View() para o usuário.
            return RedirectToAction ("Home", "Pages");
        }

        /// <summary>
        /// Utilizada para saber qual tipo de comentário deverá ser listado na página Dashboard/Gerenciar do administrador.
        /// </summary>
        static string tipoGerenciar;

        /// <summary>
        /// Método que retorna a Dashboard de gerenciamento caso o usuário seja administrador.
        /// </summary>
        [HttpGet]
        public IActionResult Gerenciar () {

            // Se usuário não estiver vazio...
            if (HttpContext.Session.GetObject<UsuarioModel> ("usuarioLogado") != null) {
                //Faça...
                UsuarioModel usuario = HttpContext.Session.GetObject<UsuarioModel> ("usuarioLogado");

                if (usuario.Administrador) {

                    // Retorna somente os comentários com status igual ao da 'static string tipoGerenciar'.
                    if (tipoGerenciar == "aceito") {
                        ViewData["Comentarios"] = ComentarioRepositorio.ListarComentariosEspecifico (TiposComentario.Aceito.ToString ());
                        ViewBag.TipoComentario = "Comentários Aceitos";
                    } else if (tipoGerenciar == "recusado") {
                        ViewData["Comentarios"] = ComentarioRepositorio.ListarComentariosEspecifico (TiposComentario.Recusado.ToString ());
                        ViewBag.TipoComentario = "Comentários Recusados";
                    } else {
                        ViewData["Comentarios"] = ComentarioRepositorio.ListarComentariosEspecifico (TiposComentario.EmEspera.ToString ());
                        ViewBag.TipoComentario = "Comentários em Espera";
                    }

                    // Retorna uma página para o usuário.
                    return View ();
                } else {
                    return RedirectToAction ("Home", "Pages");
                }
            } else {
                // Caso o usuário esteja vazio, deverá redirecioná-lo para a página de Login.
                return RedirectToAction ("Login", "Usuario");
            }

        }

        /// <summary>
        /// Método criado para editar o status do comentário, entre aprovado ou recusado.
        /// </summary>
        /// <param name="form">Se o comentário foi aceito ou recusado.</param>
        /// <returns>Retorna para a View();</returns>
        [HttpPost]
        public IActionResult Gerenciar (IFormCollection form) {
            int CommentId = int.Parse (form["commentId"]);

            ComentarioModel comentarioModel = ComentarioRepositorio.BuscarPorId (CommentId);

            if (form["choice"] == "aceito") {
                ComentarioRepositorio.Editar (TiposComentario.Aceito.ToString (), comentarioModel);
            } else {
                if (form["choice"] == "recusado") {
                    ComentarioRepositorio.Editar (TiposComentario.Recusado.ToString (), comentarioModel);
                } else {
                    ViewBag.Mensagem = "Opção inválida!";
                    return View ();
                }
            }

            ViewBag.Mensagem = $"Status do comentário de Id '{comentarioModel.Id}' foi 💕🐱‍💻😉🌹🐱‍🐉🐱‍👓🐱‍👓🤔🤔🍣!";

            return RedirectToAction ("Gerenciar");
        }

        /// <summary>
        /// Retorna a View() para a página onde todos os depoimentos se encontram.
        /// </summary>
        [HttpGet]
        public IActionResult Todos () {

            if (HttpContext.Session.GetObject<UsuarioModel> ("usuarioLogado") != null) {
                UsuarioModel usuario = HttpContext.Session.GetObject<UsuarioModel> ("usuarioLogado");

                string[] nomes = usuario.Nome.Split (" ");

                ViewBag.UsuarioLogado = nomes[0];
            } else {
                ViewBag.UsuarioLogado = null;
            }

            /// <summary>
            /// Retorna somente os comentários com status igual ao passado pelo parâmetro.
            /// </summary>
            /// <returns></returns>
            ViewData["ComentariosAceitos"] = ComentarioRepositorio.ListarComentariosEspecifico (TiposComentario.Aceito.ToString ());

            return View ();
        }

        /// <summary>
        /// Coloca dentro da variável estática 'tipoGerenciar' o tipo de comentário que deve ser enviado para a View()/[HttpGet] da página Gerenciar.    
        /// </summary>
        /// <param name="form"></param>
        /// <returns>Retorna para a View()/[HttpGet] da página Gerenciar.</returns>
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