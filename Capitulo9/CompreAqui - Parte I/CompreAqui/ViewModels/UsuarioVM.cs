using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompreAqui.ViewModels
{
    public class UsuarioVM : ViewModelBase
    {
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                NotificarAlteracao("Email");
            }
        }

        private string _nome;

        public string Nome
        {
            get { return _nome; }
            set
            {
                _nome = value;
                NotificarAlteracao("Nome");
            }
        }

        private string _senha;

        public string Senha
        {
            get { return _senha; }
            set
            {
                _senha = value;
                NotificarAlteracao("Senha");
            }
        }

        private string _confirmacaoSenha;

        public string ConfirmacaoSenha
        {
            get { return _confirmacaoSenha; }
            set
            {
                _confirmacaoSenha = value;
                NotificarAlteracao("ConfirmacaoSenha");
            }
        }

        private bool _entrarAutomaticamente = true;

        private string _senhaAntesAlteracao;

        public string SenhaAntesAlteracao
        {
            get { return _senhaAntesAlteracao; }
            set
            {
                _senhaAntesAlteracao = value;
                NotificarAlteracao("SenhaAntesAlteracao");
            }
        }

        public bool EntrarAutomaticamente
        {
            get { return _entrarAutomaticamente; }
            set
            {
                _entrarAutomaticamente = value;
                NotificarAlteracao("EntrarAutomaticamente");
            }
        }

        public string ValidarCamposCadastro()
        {
            StringBuilder validacoes = new StringBuilder();

            if (string.IsNullOrEmpty(Email))
                validacoes.AppendLine("- É necessário preencher o campo Email");

            if (string.IsNullOrEmpty(Nome))
                validacoes.AppendLine("- É necessário preencher o campo Usuário");

            if (string.IsNullOrEmpty(Senha) || string.IsNullOrEmpty(ConfirmacaoSenha))
                validacoes.AppendLine("- É necessário preencher os campos Senha e Confirmação de Senha");
            else
                if (Senha != ConfirmacaoSenha)
                    validacoes.AppendLine("- Os campos Senha e Confirmação de Senha estão com valores diferentes");

            return validacoes.ToString();
        }

        public string ValidarCamposAutenticar()
        {
            StringBuilder validacoes = new StringBuilder();

            if (string.IsNullOrEmpty(Nome))
                validacoes.AppendLine("- É necessário preencher o campo Usuário");

            if (string.IsNullOrEmpty(Senha))
                validacoes.AppendLine("- É necessário preencher o campo Senha");

            return validacoes.ToString();
        }

        public string ValidarCamposTrocaSenha()
        {
            StringBuilder validacoes = new StringBuilder();

            if (string.IsNullOrEmpty(SenhaAntesAlteracao) )
                validacoes.AppendLine("- É necessário preencher os campos Senha Atual");

            if (string.IsNullOrEmpty(Senha) || string.IsNullOrEmpty(ConfirmacaoSenha))
                validacoes.AppendLine("- É necessário preencher os campos Nova senha e Confirmação de nova senha");
            else
                if (Senha != ConfirmacaoSenha)
                    validacoes.AppendLine("- Os campos Nova senha e Confirmação de nova senha estão com valores diferentes");

            return validacoes.ToString();
        }
    }
}
