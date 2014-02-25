using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CompreAqui.ViewModels;
using CompreAqui.Modelos;
using CompreAqui.Resources;

namespace CompreAqui.Paginas
{
    public partial class Entrar : PhoneApplicationPage
    {
        private UsuarioVM _usuarioVM;

        public Entrar()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_usuarioVM == null)
                _usuarioVM = new UsuarioVM();
            this.DataContext = _usuarioVM;
        }

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string validacoes = _usuarioVM.ValidarCamposAutenticar();
            if (string.IsNullOrEmpty(validacoes))
            {
                Usuario usuario = Usuario.ValidarAutenticacao(_usuarioVM.Nome, _usuarioVM.Senha);
                if (usuario != null)
                {
                    if (_usuarioVM.EntrarAutomaticamente)
                    {
                        usuario.EntrarAutomaticamente = true;
                        usuario.AtualizarDados();
                    }

                    usuario.Autenticar();
                    NavigationService.Navigate(new Uri("/Paginas/ProdutosHub.xaml", UriKind.Relative));
                }
                else
                {
                    MostrarMensagem("- Não foi encontrado um usuário com este nome e senha");
                }
            }
            else
            {
                MostrarMensagem(validacoes);
            }
        }

        private void MostrarMensagem(string motivos)
        {
            string mensagem = "Não foi possível entrar com sua conta por um ou mais motivos abaixo";
            MessageBox.Show(string.Concat(mensagem, Environment.NewLine, motivos));
        }


    }
}