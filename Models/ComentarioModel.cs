using System;

namespace CheckPoint.Sistema.Models
{
    [Serializable]
    public class ComentarioModel
    {
        public int Id { get; set; }

        // public int IdUsuario { get; set; }
        
        public string Mensagem { get; set; }

        public DateTime DataCriacao { get; set; }

        public string Status { get; set; } // 'EmEspera', 'Aprovado', 'Recusado'.

        public int Nota { get; set; }

        //Criar propriedade UsuarioModel
        public UsuarioModel Usuario { get; set; }

        public ComentarioModel(string msg, DateTime data, string status, int nota, UsuarioModel usuario)
        {
            this.Mensagem = msg;
            this.DataCriacao = data;
            this.Status = status;
            this.Nota = nota;
            this.Usuario = usuario;
            // Instanciando UsuarioModel
        }

        public ComentarioModel(int id, string msg, DateTime data, string status, int nota, UsuarioModel usuario)
        {
            this.Id = id;
            this.Mensagem = msg;
            this.DataCriacao = data;
            this.Status = status;
            this.Nota = nota;
            this.Usuario = usuario;
        }
    }
}