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
using CompreAqui.ViewModels;

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
                                      select new CategoriaVM
                                      {
                                          Id = produtos.Categoria.Id,
                                          Descricao = produtos.Categoria.Descricao
                                      }).Distinct().ToList();

            Promocoes.ItemsSource = (from produtos in Loja.Dados.Produtos
                                     where produtos.PrecoPromocao != 0
                                     select new ProdutoVM
                                     {
                                         Id = produtos.Id,
                                         Descricao = produtos.Descricao,
                                         Preco = produtos.Preco,
                                         PrecoPromocao = produtos.PrecoPromocao,
                                         Icone = produtos.Icone
                                     }).OrderByDescending(produto => produto.Desconto)
                                       .ToList();

            Produtos.ItemsSource = (from produtos in Loja.Dados.Produtos
                                    where produtos.Id == 3 || produtos.Id == 4
                                    select new ProdutoVM
                                    {
                                        Id = produtos.Id,
                                        Descricao = produtos.Descricao,
                                        Icone = produtos.Icone,
                                        Preco = produtos.Preco,
                                        PrecoPromocao = produtos.PrecoPromocao
                                    }).ToList();
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
    }
}