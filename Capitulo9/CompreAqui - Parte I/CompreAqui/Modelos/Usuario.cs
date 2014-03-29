using CompreAqui.Resources;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompreAqui.Modelos
{
    [ Table(Name="Usuarios") ]
    public class Usuario
    {

        [Column(IsPrimaryKey=true,IsDbGenerated=true)]
        public int Id { get; set; }


        [Column]
        public string Email { get; set; }

        [Column]
        public string NomeUsuario { get; set; }

        [Column]
        public string Senha { get; set; }

        [Column]
        public bool EntrarAutomaticamente { get; set; }

        public void Autenticar()
        {
            IsolatedStorageSettings configuracoes = IsolatedStorageSettings.ApplicationSettings;
            if (configuracoes.Contains("usuarioId"))
            {
                configuracoes["usuarioId"] = this.Id;
            }
            else
            {
                configuracoes.Add("usuarioId", this.Id);
            }

            configuracoes.Save();
        }

        public void AtualizarDados()
        {
            using (BancoDados bancoDados = new BancoDados(BancoDados.StringConexao))
            {
                Usuario usuarioAntigo = bancoDados.Usuarios.FirstOrDefault(usuario => usuario.Id == this.Id);

                usuarioAntigo.EntrarAutomaticamente = this.EntrarAutomaticamente;
                usuarioAntigo.Email = this.Email;
                usuarioAntigo.NomeUsuario = this.NomeUsuario;
                usuarioAntigo.Senha = this.Senha;

                bancoDados.SubmitChanges();
            }
        }

        public static Usuario ValidarAutenticacao(string nomeUsuario, string senha)
        { 
            using( BancoDados bancoDados = new BancoDados(BancoDados.StringConexao) )
            {
                return bancoDados.Usuarios
                                 .FirstOrDefault(usuario => 
                                  usuario.NomeUsuario == nomeUsuario && 
                                  usuario.Senha == senha);
            }
        }

    }
}
