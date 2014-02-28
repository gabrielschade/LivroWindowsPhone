using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompreAqui.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        protected void NotificarAlteracao(string propriedade)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propriedade));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
