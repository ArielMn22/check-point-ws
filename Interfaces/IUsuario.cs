using CheckPoint.Sistema.Models;
using Microsoft.AspNetCore.Http;

namespace CheckPoint.Sistema.Interfaces
{
    public interface IUsuario
    {
        void Cadastrar(UsuarioModel usuario);
        int ValidaUsuario(IFormCollection form);
        UsuarioModel Login(string email, string senha);
    }
}