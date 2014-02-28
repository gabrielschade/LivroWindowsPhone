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
using System.IO.IsolatedStorage;

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
            IsolatedStorageSettings configuracoes = IsolatedStorageSettings.ApplicationSettings;
            if (configuracoes.Contains("usuarioId") &&
                Convert.ToInt32(configuracoes["usuarioId"]) != 0)
            {
                NavigationService.Navigate(new Uri("/Paginas/SuaConta.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Ops, desculpe, mas você não pode acessar esta página sem estar autenticado no aplicativo.");
                NavigationService.Navigate(new Uri("/Paginas/Entrar.xaml", UriKind.Relative));
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Categorias.ItemsSource = (from produtos in Loja.Dados.Produtos
                                      select new
                                      {
                                          Id = produtos.Categoria.Id,
                                          Descricao = produtos.Categoria.Descricao
                                      }).Distinct().ToList();

            Promocoes.ItemsSource = Loja.Dados.Produtos.Where(produto => produto.PrecoPromocao != 0)
                                                       .OrderByDescending(produto => produto.Desconto)
                                                       .ToList();

            Produtos.ItemsSource = Loja.Dados.Produtos.Skip(2).Take(2).ToList();
        }

        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsolatedStorageSettings configuracoes = IsolatedStorageSettings.ApplicationSettings;
            if (configuracoes.Contains("usuarioId") &&
                Convert.ToInt32(configuracoes["usuarioId"]) != 0)
            {
                MessageBoxResult resultado = MessageBox.Show("Deseja realmente sair do aplicativo?", "Confirmação", MessageBoxButton.OKCancel);
                if (resultado == MessageBoxResult.OK)
                {
                    Application.Current.Terminate();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void TextBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Paginas/Produtos.xaml", UriKind.Relative));
        }

        private void Categoria_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TextBlock categoriaClicada = sender as TextBlock;

            string categoria = categoriaClicada.Text;
            string categoriaId = Convert.ToString(categoriaClicada.Tag);
            string parametros = string.Format("?categoria={0}&categoriaId={1}", categoria, categoriaId);

            NavigationService.Navigate(new Uri(string.Concat("/Paginas/Produtos.xaml", parametros), UriKind.Relative));
        }

    }
}