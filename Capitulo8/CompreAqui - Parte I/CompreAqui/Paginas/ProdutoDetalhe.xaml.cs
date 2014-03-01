using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CompreAqui.Modelos;

namespace CompreAqui.Paginas
{
    public partial class ProdutoDetalhe : PhoneApplicationPage
    {
        public ProdutoDetalhe()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string id;
            Produto produto;

            NavigationContext.QueryString.TryGetValue("id", out id);
            if (!string.IsNullOrEmpty(id))
            { 
                //DataContext = Loja.Dados.Produtos.FirstOrDefault()
            }


        }

    }
}