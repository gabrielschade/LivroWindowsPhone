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
using System.Threading.Tasks;
using CompreAqui.Auxiliar;
using Newtonsoft.Json;
using CompreAqui.ViewModels;
using System.Windows.Input;

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (Loja.Dados.Produtos == null)
            {

                Task thread = CarregarDados();
                thread.ContinueWith(
                (resultado) =>
                {
                    Dispatcher.BeginInvoke(VincularDados);
                });
            }
            else
            {
                VincularDados();
            }
        }

        private void VincularDados()
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

        private async Task CarregarDados()
        {
            string dados = LeitorArquivo.Ler("/CompreAqui;component/Resources/dados.txt");
            Loja.Dados = JsonConvert.DeserializeObject<Loja>(dados);
            await Task.Delay(3000);
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
            string parametros = string.Format("?titulo={0}&categoriaId={1}", categoria, categoriaId);

            NavigationService.Navigate(new Uri(string.Concat("/Paginas/Produtos.xaml", parametros), UriKind.Relative));
        }

        private void Produto_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Grid componentePressionado = sender as Grid;
            if (componentePressionado != null)
            {
                string id = Convert.ToString(componentePressionado.Tag);
                string parametros = string.Format("?id={0}", id);

                NavigationService.Navigate(new Uri(string.Concat("/Paginas/ProdutoDetalhe.xaml", parametros), UriKind.Relative));
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            if (PainelPesquisa.Visibility == System.Windows.Visibility.Collapsed)
            {
                PainelPesquisa.Visibility = System.Windows.Visibility.Visible;
                CampoPesquisa.Focus();
            }
            else
            {
                ExecutarPesquisa(); 
            }
        }

        private void CampoPesquisa_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                ExecutarPesquisa();
            }
        }

        private void ExecutarPesquisa()
        {
            string parametros = string.Format("?titulo={0}&pesquisa={1}", "resultados", CampoPesquisa.Text);

            NavigationService.Navigate(new Uri(string.Concat("/Paginas/Produtos.xaml", parametros), UriKind.Relative));
            DesaparecerPainelPesquisa();
        }

        private void CampoPesquisa_LostFocus(object sender, RoutedEventArgs e)
        {
            DesaparecerPainelPesquisa();
        }

        private void DesaparecerPainelPesquisa()
        {
            CampoPesquisa.Text = string.Empty;
            PainelPesquisa.Visibility = System.Windows.Visibility.Collapsed;
        }

    }
}