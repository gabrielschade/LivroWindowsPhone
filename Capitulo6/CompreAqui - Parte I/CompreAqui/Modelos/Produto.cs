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

        public string DescricaoDetalhada { get; set; }

        public double AvaliacaoMedia { get; set; }

        public Categoria Categoria { get; set; }

        public string IconName { get; set; }
    }
}
