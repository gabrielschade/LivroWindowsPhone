using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompreAqui.ViewModels
{
    public class CategoriaVM : ViewModelBase
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is CategoriaVM)
                return Id == ((CategoriaVM)obj).Id;

            return false;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}
