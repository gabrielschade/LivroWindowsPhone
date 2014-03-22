using CompreAqui.Modelos;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompreAqui.Resources
{
    public class BancoDados : DataContext
    {
        public BancoDados(string stringConexao)
            : base(stringConexao)
        {}

        public static string StringConexao 
        {
            get 
            {
                return "isostore:/compreaqui.sdf";
            }
        }

        public Table<Usuario> Usuarios;
    }
}
