using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompreAqui.Modelos
{
    public class Produto
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public double Preco { get; set; }

        public double PrecoPromocao { get; set; }

        public double PrecoAPagar
        {
            get
            {
                double valor;
                if (PrecoPromocao != 0)
                    valor = PrecoPromocao;
                else
                    valor = Preco;

                return Math.Round(valor, 2);
            }
        }

        public string DescricaoDetalhada { get; set; }

        public double AvaliacaoMedia { get; set; }

        public Categoria Categoria { get; set; }

        public string Icone { get; set; }

        public double Desconto
        {
            get
            {
                return Math.Round(100 - (PrecoPromocao * 100 / Preco), 2);
            }
        }
    }
}
