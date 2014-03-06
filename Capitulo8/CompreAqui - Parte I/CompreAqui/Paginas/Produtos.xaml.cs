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

            string titulo, categoriaId, pesquisa;

            NavigationContext.QueryString.TryGetValue("titulo", out titulo);
            NavigationContext.QueryString.TryGetValue("categoriaId", out categoriaId);
            NavigationContext.QueryString.TryGetValue("pesquisa", out pesquisa);

            if (!string.IsNullOrEmpty(titulo))
                Titulo.Text = titulo.ToLower();

            if (!string.IsNullOrEmpty(categoriaId))
                produtos = produtos.Where(produto => produto.CategoriaId == Convert.ToInt32(categoriaId)).ToList();

            if (!string.IsNullOrEmpty(pesquisa))
                produtos = produtos.Where(produto => produto.Descricao.ToLower().Contains(pesquisa.ToLower())).ToList();

            Listagem.ItemsSource = produtos;
            if (produtos.Count == 0)
            {
                CampoMensagem.Visibility = System.Windows.Visibility.Visible;
            }
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


    }
}