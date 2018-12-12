using System.Collections.Generic;
using CheckPoint.Sistema.Models;

namespace CheckPoint.Sistema.Interfaces
{
    public interface IComentario
    {
        void Cadastrar(ComentarioModel comentario);
        List<ComentarioModel> ListarComentariosEspecifico(string status);
        ComentarioModel BuscarPorId(int id);
        void Editar(string newStatus, ComentarioModel newComentario);
    }
}