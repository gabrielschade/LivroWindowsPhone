using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CompreAqui.Resources;
using CompreAqui.Modelos;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Resources;
using CompreAqui.Auxiliar;
using System.IO.IsolatedStorage;

namespace CompreAqui
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IsolatedStorageSettings configuracoes = IsolatedStorageSettings.ApplicationSettings;
            if (configuracoes.Contains("usuarioId") &&
                Convert.ToInt32(configuracoes["usuarioId"]) != 0)
            {
                using (BancoDados dados = new BancoDados(BancoDados.StringConexao))
                {
                    Usuario ultimoUsuario = dados.Usuarios.FirstOrDefault(usuario => usuario.Id == Convert.ToInt32(configuracoes["usuarioId"]));
                    if (ultimoUsuario.EntrarAutomaticamente)
                        NavigationService.Navigate(new Uri("/Paginas/ProdutosHub.xaml", UriKind.Relative));
                    else
                        configuracoes["usuarioId"] = 0;
                }
            }

            base.OnNavigatedTo(e);
        }

    }
}