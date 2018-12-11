using System;

namespace CheckPoint.Sistema.Models
{
    [Serializable]
    public class UsuarioModel
    {
        #region Propriedades
        public int Id { get; set; }
        
        public string Nome { get; set; }

        public string  Email { get; set; }

        public string  Senha { get; set; }
    
        public bool Administrador { get; set; }
        #endregion

        #region Métodos construtores
        public UsuarioModel(){
            
        }
        public UsuarioModel(int id, string nome, string email, string senha, bool admin)
        {
            this.Id = id;
            this.Nome = nome;
            this.Email = email;
            this.Senha = senha;
            this.Administrador = admin;
        }

        public UsuarioModel(string nome, string email, string senha, bool admin)
        {
            // O 'id' será definido no repositório;
            this.Nome = nome;
            this.Email = email;
            this.Senha = senha;
            this.Administrador = admin;
        }
        #endregion
    }
}