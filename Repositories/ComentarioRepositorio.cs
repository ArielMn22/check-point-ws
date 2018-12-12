using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CheckPoint.Sistema.Interfaces;
using CheckPoint.Sistema.Models;
using Microsoft.AspNetCore.Http;

namespace CheckPoint.Sistema.Repositories {
    public class ComentarioRepositorio : IComentario {
        private List<ComentarioModel> ComentariosSalvos { get; set; } // Possui uma lista dos comentários cadastrados no sistema;
        public ComentarioRepositorio () {
            if (File.Exists ("comentarios.dat")) {
                ComentariosSalvos = LerArquivoSerializado ();
            } else {
                ComentariosSalvos = new List<ComentarioModel> ();
            }
        }

        public void Cadastrar (ComentarioModel comentario) {
            // Adiciona o ID do comentário com base na quantidade de comentários salvos na lista.
            comentario.Id = ComentariosSalvos.Count + 1;

            // Adiciona o comentário a lista de comentários;
            ComentariosSalvos.Add (comentario);

            EscreverNoArquivo ();
        }

        private void EscreverNoArquivo () {
            // Irá armazenar os dados da lista quando ela for serializada.
            MemoryStream memoria = new MemoryStream ();

            // Objeto que fará a serialização.
            BinaryFormatter serializador = new BinaryFormatter ();

            // Serializa os dados para a 'memoria'.
            serializador.Serialize (memoria, ComentariosSalvos);

            // Converte os dados para uma array de 'bytes'.
            byte[] bytes = memoria.ToArray ();

            //Escreve o 'array' de bytes no 'comentarios.dat'.
            File.WriteAllBytes ("comentarios.dat", bytes);
        }

        public List<ComentarioModel> LerArquivoSerializado () {
            // Lê os bytes do arquivo.dat
            byte[] bytesSerializados = File.ReadAllBytes ("comentarios.dat");

            // Cria o fluxo de memória com os bytes do arquivo serializado;
            MemoryStream memoria = new MemoryStream (bytesSerializados);

            // Cria o serializar para poder 'serializar' e 'deserializar' arquivos;
            BinaryFormatter serializador = new BinaryFormatter ();

            // Deserializa os dados para gerar a lista;
            return (List<ComentarioModel>) serializador.Deserialize (memoria);
        }

        public ComentarioModel BuscarPorId (int id) {
            foreach (ComentarioModel comentario in ComentariosSalvos) {
                if (id == comentario.Id) {
                    return comentario;
                }
            }

            return null;
        }

        public void Editar (string newStatus, ComentarioModel newComentario) {
            for (int i = 0; i < ComentariosSalvos.Count; i++) {
                if (newComentario.Id == ComentariosSalvos[i].Id)
                {
                    ComentariosSalvos[i].Status = newStatus;

                    EscreverNoArquivo();

                    break;
                }
            }

        }

        public List<ComentarioModel> ListarComentarios() => ComentariosSalvos;
        
        public List<ComentarioModel> ListarComentariosEspecifico(string status)
        {
            List<ComentarioModel> comentariosEspecificos = new List<ComentarioModel>();

            foreach(ComentarioModel comentario in ComentariosSalvos)
            {
                if (comentario.Status == status)
                {
                    comentariosEspecificos.Add(comentario);
                }
            }

            comentariosEspecificos.Reverse();

            return comentariosEspecificos;
        }
    }
}