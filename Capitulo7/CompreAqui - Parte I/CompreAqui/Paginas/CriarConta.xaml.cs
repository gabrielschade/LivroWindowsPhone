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
using System.Text;
using CompreAqui.Resources;
using CompreAqui.Modelos;
using System.IO.IsolatedStorage;

namespace CompreAqui.Paginas
{
    public partial class CriarConta : PhoneApplicationPage
    {

        private UsuarioVM _usuarioVM;

        public CriarConta()
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
            string validacoes = _usuarioVM.ValidarCamposCadastro();
            if (!string.IsNullOrEmpty(validacoes))
            {
                string mensagem = string.Concat(
                    "Não foi possível gravar esta conta por um ou mais motivos abaixo:", Environment.NewLine, validacoes);

                MessageBox.Show(mensagem);
            }
            else
            {
                GravarUsuario();
            }
        }

        private void GravarUsuario()
        {
            Usuario novoUsuario = new Usuario();

            novoUsuario.Email = _usuarioVM.Email;
            novoUsuario.NomeUsuario = _usuarioVM.Nome;
            novoUsuario.Senha = _usuarioVM.Senha;
            novoUsuario.EntrarAutomaticamente = _usuarioVM.EntrarAutomaticamente;

            using (BancoDados bancoDados = new BancoDados(BancoDados.StringConexao))
            {
                bancoDados.Usuarios.InsertOnSubmit(novoUsuario);
                try
                {
                    bancoDados.SubmitChanges();
                    novoUsuario.Autenticar();
                    NavigationService.Navigate(new Uri("/Paginas/ProdutosHub.xaml", UriKind.Relative));
                }
                catch
                {
                    MessageBox.Show("Houve um problema ao tentar criar sua conta, tente novamente mais tarde.");
                }
            }
        }

    }
}

