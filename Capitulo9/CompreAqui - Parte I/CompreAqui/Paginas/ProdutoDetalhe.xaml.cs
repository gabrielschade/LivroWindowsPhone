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
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;

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
            CarregarDadosAsync();
        }

        private async void CarregarDadosAsync()
        {
            if (Loja.Dados.Produtos == null)
            {
                CampoMensagem.Text = "Carregando dados...";
                AlterarCamposCarregando(System.Windows.Visibility.Visible);
                txtTitulo.Text = "Aguarde";
                await Loja.Dados.CarregarDados();
            }

            AlterarCamposCarregando(System.Windows.Visibility.Collapsed);

            VincularDados();
        }

        private void AlterarCamposCarregando(System.Windows.Visibility visibilidade)
        {
            CampoMensagem.Visibility = visibilidade;
            BarraProgresso.Visibility = visibilidade;
            if (visibilidade == System.Windows.Visibility.Visible)
                PainelConteudo.Visibility = System.Windows.Visibility.Collapsed;
            else
                PainelConteudo.Visibility = System.Windows.Visibility.Visible;
        }

        private void VincularDados()
        {
            string id;
            ProdutoVM produto = null;

            NavigationContext.QueryString.TryGetValue("id", out id);
            if (!string.IsNullOrEmpty(id))
            {
                produto = (from produtos in Loja.Dados.Produtos
                           where produtos.Id == Convert.ToInt32(id)
                           select new ProdutoVM
                           {
                               Id = produtos.Id,
                               AvaliacaoMedia = produtos.AvaliacaoMedia,
                               CategoriaDescricao = produtos.Categoria.Descricao,
                               Descricao = produtos.Descricao,
                               DescricaoDetalhada = produtos.DescricaoDetalhada,
                               Icone = produtos.Icone,
                               Preco = produtos.Preco,
                               PrecoPromocao = produtos.PrecoPromocao
                           }).FirstOrDefault();
            }

            if (produto != null)
            {
                DataContext = produto;
            }
            else
            {
                MessageBox.Show("Não foi possível encontrar o produto", "Alerta", MessageBoxButton.OK);
                NavigationService.GoBack();
            }
        }

        private void Fixar_Click(object sender, EventArgs e)
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

            string url = string.Concat("/Paginas/ProdutoDetalhe.xaml", parametros.ToString());
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
            ProdutoVM dataContext = DataContext as ProdutoVM;


            data.Title = dataContext.Descricao;
            data.BackgroundImage = new Uri("/Assets/Images/tileBackground.png", UriKind.Relative);

            data.BackTitle = dataContext.Descricao;
            data.BackContent = string.Concat(dataContext.PrecoAPagar.ToString(), " R$");
            data.BackBackgroundImage = new Uri("/Assets/Images/tileBackground.png", UriKind.Relative);

            return data;
        }

        private void PaginaInicial_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void Compartilhar_Click(object sender, EventArgs e)
        {
            ProdutoVM dataContext = DataContext as ProdutoVM;
            ShareStatusTask launcherCompartilhar = new ShareStatusTask();
            launcherCompartilhar.Status = 
                string.Concat("Confiram o produto ", dataContext.Descricao, 
                              " no aplicativo CompreAqui, está custando apenas ", 
                              dataContext.PrecoAPagar, " R$.");

            launcherCompartilhar.Show();
        }

        private void Comprar_Click(object sender, EventArgs e)
        {
            IsolatedStorageSettings configuracoes = IsolatedStorageSettings.ApplicationSettings;
            if (configuracoes.Contains("usuarioId") &&
                Convert.ToInt32(configuracoes["usuarioId"]) != 0)
            {
                NavigationService.Navigate(new Uri("/Paginas/FinalizarCompra.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Ops, desculpe, mas você não pode efetuar uma compra sem estar autenticado no aplicativo.");
                NavigationService.Navigate(new Uri("/Paginas/Entrar.xaml", UriKind.Relative));
            }
            
        }

    }
}