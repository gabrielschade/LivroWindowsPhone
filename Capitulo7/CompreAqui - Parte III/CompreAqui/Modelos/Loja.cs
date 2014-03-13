using CompreAqui.Auxiliar;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompreAqui.Modelos
{
    public class Loja
    {
        private Loja()
        {}

        private static Loja dados;
        public static Loja Dados
        {
            get
            {
                if (dados == null)
                    dados = new Loja();

                return dados;
            }
            set
            {
                dados = value;
            }
        }

        public List<Produto> Produtos { get; set; }

        public async Task CarregarDadosAsync()
        {
            string dados = LeitorArquivo.Ler("/CompreAqui;component/Resources/dados.txt");
            Loja.Dados = JsonConvert.DeserializeObject<Loja>(dados);
            await Task.Delay(3000);
        }
    }
}
