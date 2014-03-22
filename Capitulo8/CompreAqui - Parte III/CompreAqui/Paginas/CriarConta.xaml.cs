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
                    AtualizarLiveTile(novoUsuario.NomeUsuario, novoUsuario.Email);
                }
                catch
                {
                    MessageBox.Show("Houve um problema ao tentar criar sua conta, tente novamente mais tarde.");
                }
            }
        }

        private void AtualizarLiveTile(string nomeUsuario, string email)
        {
            ShellTile appTile = ShellTile.ActiveTiles.FirstOrDefault();
            if (appTile != null)
            {
                IconicTileData dadosTile = new IconicTileData();
                dadosTile.WideContent1 = nomeUsuario;
                dadosTile.WideContent2 = email;

                appTile.Update(dadosTile);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back)
            {
                State["email"] = _usuarioVM.Email;
                State["usuario"] = _usuarioVM.Nome;
                State["senha"] = _usuarioVM.Senha;
                State["confirmacao"] = _usuarioVM.ConfirmacaoSenha;
                State["lembrarme"] = _usuarioVM.EntrarAutomaticamente;
            }

            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (_usuarioVM == null)
            {
                _usuarioVM = new UsuarioVM();

                if (State.ContainsKey("email"))
                    _usuarioVM.Email = State["email"] as string;

                if (State.ContainsKey("usuario"))
                    _usuarioVM.Nome = State["usuario"] as string;

                if (State.ContainsKey("senha"))
                    _usuarioVM.Senha = State["senha"] as string;

                if (State.ContainsKey("confirmacao"))
                    _usuarioVM.ConfirmacaoSenha = State["confirmacao"] as string;

                if (State.ContainsKey("lembrarme"))
                    _usuarioVM.EntrarAutomaticamente = Convert.ToBoolean(State["lembrarme"]);
            }

            this.DataContext = _usuarioVM;
        }


    }
}

