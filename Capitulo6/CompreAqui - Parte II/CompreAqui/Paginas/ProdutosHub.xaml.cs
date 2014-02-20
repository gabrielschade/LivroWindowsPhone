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
    public partial class ProdutosHub : PhoneApplicationPage
    {
        public ProdutosHub()
        {
            InitializeComponent();
        }

        private void SuaConta_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Paginas/SuaConta.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Categorias.ItemsSource = (from produtos in Loja.Dados.Produtos
                                      select new
                                      {
                                          Id = produtos.Categoria.Id,
                                          Descricao = produtos.Categoria.Descricao
                                      }).Distinct().ToList();

            Promocoes.ItemsSource = Loja.Dados.Produtos.Where(produto => produto.PrecoPromocao != 0).ToList();
        }
    }
}