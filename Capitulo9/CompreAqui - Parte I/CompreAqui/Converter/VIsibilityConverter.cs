using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CompreAqui.Converter
{
    public class VisibilityConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double valor = System.Convert.ToDouble(value);

            System.Windows.Visibility visibilidade;

            if (valor != 0)
                visibilidade = System.Windows.Visibility.Visible;
            else
                visibilidade = System.Windows.Visibility.Collapsed;

            return visibilidade;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
