using AppRpgEtect.Helpers.Message;
using AppRpgEtect.Models;
using AppRpgEtect.Services.Usuarios;
using AppRpgEtect.Views.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppRpgEtect.ViewModels.Usuarios
{
    public class UsuarioViewModel : BaseViewModel
    {
        private UsuarioService uService;
        public ICommand RegistrarCommand { get; set; }
        public ICommand AutenticarCommand { get; set; }
        public ICommand DirecionarCadastroCommand { get; set; }

        public UsuarioViewModel()
        {
            uService= new UsuarioService();
            InicializarCommands();
        }

        public void InicializarCommands()
        {
            RegistrarCommand = new Command(async () => await RegistrarUsuario());
            AutenticarCommand = new Command(async () => await AutenticarUsuario());
            DirecionarCadastroCommand = new Command(async () => await DirecionarParaCadastro());
        }

        #region AtributosPropriedades

        private string login = string.Empty;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnProertyChanged();
            }
        }

        private string senha = string.Empty;

        public string Senha
        {
            get { return senha; }
            set
            {
                senha = value;
                OnProertyChanged();
            }
        }
        #endregion

        #region Métodos
        public async Task RegistrarUsuario()
        {
            try
            {
                //Próxima codificação aqui
                Usuario u = new Usuario();
                u.Username = login;
                u.PasswordString = senha;

                Usuario uRegistrado = await uService.PostRegistrarUsuarioAsync(u);

                if (uRegistrado.Id != 0)
                {
                    string mensagem = $"Usuário Id {uRegistrado.Id} registrado com successo.";
                    await Application.Current.MainPage.DisplayAlert("Informação", mensagem, "Ok");

                    await Application.Current.MainPage
                        .Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + "Detalhes:" + ex.InnerException, "Ok");
            }
        }

        public async Task AutenticarUsuario()
        {
            try
            {
                Usuario u = new Usuario();
                u.Username = Login;
                u.PasswordString = senha;

                Usuario uAutenticado = await uService.PostAutenticarUsuarioAsync(u);

                if (!string.IsNullOrEmpty(uAutenticado.Token))
                {
                    string mensagem = $"Bem-vindo(a) {uAutenticado.Username}.";

                    Preferences.Set("UsuarioId", uAutenticado.Id);
                    Preferences.Set("UsuarioUsername", uAutenticado.Username);
                    Preferences.Set("UsuarioPerfil", uAutenticado.Perfil);
                    Preferences.Set("UsuarioToken", uAutenticado.Token);

                    Models.Email email= new Models.Email();
                    email.Remetente = "araujodasilvaluisgustavo@gmail.com";
                    email.RemetentePassword = "qvncovwzbhdoysns";
                    email.Destinatario = "araujodasilvaluisgustavo@gmail.com";
                    email.DestinatarioCopia = "araujodasilvaluisgustavo@gmail.com";
                    email.DominioPrimario = "smtp.gmail.com";
                    email.PortaPrimaria = 587;
                    email.Assunto = "Notificação de Acesso";
                    email.Mensagem = $"Usuario {u.Username} acessou o aplicativo +" +
                        $"em {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

                    EmailHelper emailHelper= new EmailHelper();
                    await emailHelper.EnviarEmail(email);







                    await Application.Current.MainPage
                        .DisplayAlert("Informação", mensagem, "Ok");

                    Application.Current.MainPage = new AppShell();

                }
                else
                {
                    await Application.Current.MainPage
                        .DisplayAlert("Informação", "Dados incorretos :(", "Ok");
                }
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + "Detalhes" + ex.InnerException, "Ok");
            }
        }

        public async Task DirecionarParaCadastro()
        {
            try
            {
                await Application.Current.MainPage.
                    Navigation.PushAsync(new CadastroView());
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage
                    .DisplayAlert("Informação", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}
#endregion

