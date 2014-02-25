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
    public partial class Produtos : PhoneApplicationPage
    {
        public Produtos()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            List<Produto> produtos = Loja.Dados.Produtos;
            string categoria, categoriaId;

            NavigationContext.QueryString.TryGetValue("categoria", out categoria);
            NavigationContext.QueryString.TryGetValue("categoriaId", out categoriaId);

            if (!string.IsNullOrEmpty(categoria))
                Titulo.Text = categoria.ToLower();

            if (!string.IsNullOrEmpty(categoriaId))
                produtos = produtos.Where(produto => produto.Categoria.Id == Convert.ToInt32(categoriaId)).ToList();

            Listagem.ItemsSource = produtos;
        }
    }
}