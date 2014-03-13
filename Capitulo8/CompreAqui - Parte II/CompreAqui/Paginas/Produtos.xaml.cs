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
using CompreAqui.ViewModels;
using System.Text;

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

            CarregarDadosAsync();
            
        }

        private async void CarregarDadosAsync()
        {
            string titulo, categoriaId, pesquisa;

            NavigationContext.QueryString.TryGetValue("titulo", out titulo);
            NavigationContext.QueryString.TryGetValue("categoriaId", out categoriaId);
            NavigationContext.QueryString.TryGetValue("pesquisa", out pesquisa);

            if (!string.IsNullOrEmpty(titulo))
                Titulo.Text = titulo.ToLower();

            if (Loja.Dados.Produtos == null)
            {
                CampoMensagem.Text = "Carregando dados...";
                AlterarCamposCarregando(System.Windows.Visibility.Visible);

                await Loja.Dados.CarregarDados();
            }

            AlterarCamposCarregando(System.Windows.Visibility.Collapsed);

            List<ProdutoVM> produtos = (from produto in Loja.Dados.Produtos
                                        select new ProdutoVM
                                        {
                                            Id = produto.Id,
                                            Descricao = produto.Descricao,
                                            Preco = produto.Preco,
                                            PrecoPromocao = produto.PrecoPromocao,
                                            AvaliacaoMedia = produto.AvaliacaoMedia,
                                            CategoriaId = produto.Categoria.Id,
                                            Icone = produto.Icone
                                        }).ToList();

            if (!string.IsNullOrEmpty(categoriaId))
                produtos = produtos.Where(produto => produto.CategoriaId == Convert.ToInt32(categoriaId)).ToList();

            if (!string.IsNullOrEmpty(pesquisa))
                produtos = produtos.Where(produto => produto.Descricao.ToLower().Contains(pesquisa.ToLower())).ToList();

            Listagem.ItemsSource = produtos;
            if (produtos.Count == 0)
            {
                CampoMensagem.Visibility = System.Windows.Visibility.Visible;
                CampoMensagem.Text = "Não foi encontrado nenhum produto com o filtro inserido";
            }
        }

        private void AlterarCamposCarregando(System.Windows.Visibility visibilidade)
        {
            CampoMensagem.Visibility = visibilidade;
            BarraProgresso.Visibility = visibilidade;
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

        private void AddTo_Click(object sender, EventArgs e)
        {
            StringBuilder parametros = null;

            foreach (string parametro in NavigationContext.QueryString.Keys)
            {
                if (parametros == null)
                    parametros = new StringBuilder("?");
                else
                    parametros.Append("&");

                parametros.AppendFormat("{0}={1}", parametro, NavigationContext.QueryString[parametro]);
            }

            string url = string.Concat("/Paginas/Produtos.xaml", parametros.ToString());
            if (ShellTile.ActiveTiles.Any(tiles => tiles.NavigationUri.ToString() == url))
            {
                string mensagem = "Este atalho já está fixado em sua tela inicial";
                MessageBox.Show(string.Concat("Não foi possível fixar o tile por um ou mais motivos abaixo:", Environment.NewLine, mensagem));
            }
            else
            {
                ShellTile.Create(new Uri(url, UriKind.Relative), CriarDadosTile());
            }
        }

        private StandardTileData CriarDadosTile()
        {
            StandardTileData data = new StandardTileData();
            data.Title = Titulo.Text;
            data.BackgroundImage = new Uri("/Assets/Images/tileBackground.png", UriKind.Relative);

            if (Listagem.ItemsSource.Count > 0)
            {
                List<ProdutoVM> produtos = (List<ProdutoVM>)Listagem.ItemsSource;
                ProdutoVM produto = produtos.FirstOrDefault();

                data.Count = produtos.Count;
                data.BackTitle = produto.Descricao;
                data.BackBackgroundImage = new Uri("/Assets/Images/tileBackground.png", UriKind.Relative);

                if (NavigationContext.QueryString.ContainsKey("pesquisa"))
                    data.BackContent = string.Concat("Pesquisa: ", NavigationContext.QueryString["pesquisa"]);
                else
                    data.BackContent = Titulo.Text;
            }

            return data;
        }

        private void Home_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }


    }
}