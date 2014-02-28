using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;

namespace CompreAqui.Auxiliar
{
    public static class LeitorArquivo
    {
        public static string Ler(string caminhoArquivo)
        {
            StreamResourceInfo resourceInfo = Application.GetResourceStream(new Uri(caminhoArquivo, UriKind.Relative));
            if (resourceInfo != null)
            {
                Stream arquivo = resourceInfo.Stream;
                if (arquivo.CanRead)
                {
                    StreamReader leitor = new StreamReader(arquivo, System.Text.Encoding.GetEncoding("iso8859-1"));
                    return leitor.ReadToEnd();
                }
            }

            throw new InvalidOperationException("Problema ao ler o arquivo de dados");
        }
    }
}
