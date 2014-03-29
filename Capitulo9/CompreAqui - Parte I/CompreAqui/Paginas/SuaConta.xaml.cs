using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using CompreAqui.ViewModels;
using CompreAqui.Resources;
using CompreAqui.Modelos;
using System.Collections.ObjectModel;

namespace CompreAqui.Paginas
{
    public partial class SuaConta : PhoneApplicationPage
    {
        private UsuarioVM _usuarioVM;
        private Usuario _usuarioAtual;

        public SuaConta()
        {
            InitializeComponent();
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            MessageBoxResult resultado =
                MessageBox.Show("Tem certeza que deseja desconectar sua conta do aplicativo?", "Confirmação", MessageBoxButton.OKCancel);

            if (resultado == MessageBoxResult.OK)
            {
                IsolatedStorageSettings configuracoes = IsolatedStorageSettings.ApplicationSettings;
                configuracoes["usuarioId"] = 0;
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_usuarioVM == null)
                _usuarioVM = new UsuarioVM();
            this.DataContext = _usuarioVM;

            IsolatedStorageSettings configuracoes = IsolatedStorageSettings.ApplicationSettings;
            using (BancoDados bancoDados = new BancoDados(BancoDados.StringConexao))
                _usuarioAtual = bancoDados.Usuarios.FirstOrDefault(usuario => usuario.Id == Convert.ToInt32(configuracoes["usuarioId"]));

            if (_usuarioAtual != null)
            {
                _usuarioVM.Nome = _usuarioAtual.NomeUsuario;
                _usuarioVM.Email = _usuarioAtual.Email;
            }
        }

        private void Button_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            string validacoes = _usuarioVM.ValidarCamposTrocaSenha();
            if (string.IsNullOrEmpty(validacoes))
            {
                if (_usuarioAtual.Senha == _usuarioVM.SenhaAntesAlteracao)
                {
                    _usuarioAtual.Senha = _usuarioVM.Senha;
                    _usuarioAtual.AtualizarDados();

                    MessageBox.Show("Sua senha foi atualizada com sucesso!");
                    _usuarioVM.SenhaAntesAlteracao = string.Empty;
                    _usuarioVM.Senha = string.Empty;
                    _usuarioVM.ConfirmacaoSenha = string.Empty;
                }
                else
                    MostrarMensagem("- O campo Senha atual não está com a senha correta");
            }
            else
            {
                MostrarMensagem(validacoes);
            }
        }

        public void MostrarMensagem(string motivos)
        {
            MessageBox.Show(string.Concat("Não foi possível alterar a senha por um ou mais motivos abaixo:", Environment.NewLine, motivos));
        }

    }
}