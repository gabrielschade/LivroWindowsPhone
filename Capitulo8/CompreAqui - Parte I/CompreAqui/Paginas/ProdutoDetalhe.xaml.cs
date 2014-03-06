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

    }
}