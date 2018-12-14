using System;
using System.IO;
using System.Collections.Generic;
using CheckPoint.Sistema.Models;
using System.Runtime.Serialization.Formatters.Binary;
using CheckPoint.Sistema.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CheckPoint.Sistema.Repositories
{
    public class UsuarioRepositorio : IUsuario
    {
        private List<UsuarioModel> UsuariosSalvos { get; set; } // Possui uma lista dos usuários cadastrados no sistema;
        public UsuarioRepositorio()
        {
            if (File.Exists("usuarios.dat"))
            {
                UsuariosSalvos = LerArquivoSerializado();
            } else {
                UsuariosSalvos = new List<UsuarioModel>();
                
                UsuarioModel usuario = new UsuarioModel(
                    id: 1,
                    nome: "Admin",
                    email: "admin@carfel.com",
                    senha: "admin",
                    admin: true
                );

                UsuariosSalvos.Add(usuario); // Adiciona o usuário a lista.

                EscreverNoArquivo(); // Irá criar o usuário administrador automaticamente;
            }
        }

        public void Cadastrar(UsuarioModel usuario)
        {
            // Adiciona o ID do usuário com base na quantidade de usuários salvos na lista.
            usuario.Id = UsuariosSalvos.Count + 1;

            // Adiciona o usuário a lista de usuários;
            UsuariosSalvos.Add(usuario);

            EscreverNoArquivo();
        }

        private void EscreverNoArquivo()
        {
            // Irá armazenar os dados da lista quando ela for serializada.
            MemoryStream memoria = new MemoryStream();

            // Objeto que fará a serialização.
            BinaryFormatter serializador = new BinaryFormatter();

            // Serializa os dados para a 'memoria'.
            serializador.Serialize(memoria, UsuariosSalvos);

            // Converte os dados para uma array de 'bytes'.
            byte[] bytes = memoria.ToArray();

            //Escreve o 'array' de bytes no 'usuarios.dat'.
            File.WriteAllBytes("usuarios.dat", bytes);
        }

        public List<UsuarioModel> LerArquivoSerializado()
        {
            // Lê os bytes do arquivo.dat
            byte[] bytesSerializados = File.ReadAllBytes ("usuarios.dat");

            // Cria o fluxo de memória com os bytes do arquivo serializado;
            MemoryStream memoria = new MemoryStream (bytesSerializados);

            // Cria o serializar para poder 'serializar' e 'deserializar' arquivos;
            BinaryFormatter serializador = new BinaryFormatter ();

            // Deserializa os dados para gerar a lista;
            return (List<UsuarioModel>) serializador.Deserialize (memoria);
        }

        // Validacao

        /// <summary>
        /// Método que valida se o usuário pode ser cadastrado.
        /// </summary>
        /// <param name="form">Dados do usuário da ViewPage().</param>
        /// <returns>0 - O usuário pode ser cadastrado. 1 - Nome vazio ou nulo. 2 -  O seguinte e-mail já está cadastrado. 3 - Senhas não coincidem. 4 - Senha possui menos de 6 caracteres.</returns>
        public int ValidaUsuario(IFormCollection form)
        {
            if (string.IsNullOrEmpty(form["nome"]))
            {
                return 1; // Nome nulo ou vazio.
            }

            foreach(UsuarioModel user in UsuariosSalvos)
            {
                if (form["email"] == user.Email)
                {
                    return 2; // O seguinte e-mail já está cadastrado.
                }
            }

            if (form["senha"] != form["confirmaSenha"])
            {
                return 3; // Senhas não coincidem.
            } else {
                string senha = form["senha"];

                if (senha.Length < 5)
                {
                    return 4; // Senha possui menos de 6 caracteres.
                }
            }

            return 0; // O usuário pode ser cadastrado.
        }

        /// <summary>
        /// Método que retorna um usuário se este for cadastrado no sistema, retorna o usuário.
        /// Caso não exista, retorna 'null'.
        /// </summary>
        /// <param name="email">Email informado pelo usuário na View.</param>
        /// <param name="senha">Senha informada pelo usuário na View.</param>
        /// <returns>'UsuarioModel' caso esteja cadastrado, 'null' caso não esteja.</returns>
        // Utilizei Linq para contruir o método.
        public UsuarioModel Login(string email, string senha) => UsuariosSalvos.FirstOrDefault(usuario => usuario.Email == email && usuario.Senha == senha);

        /// <summary>
        /// Busca o usuário por ID.
        /// </summary>
        /// <returns>Retorna um usuarioModel</returns>
        public UsuarioModel BuscarPorId(int id) => UsuariosSalvos.FirstOrDefault(d => d.Id == id);
    }
}