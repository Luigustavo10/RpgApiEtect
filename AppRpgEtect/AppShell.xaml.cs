using AppRpgEtect.ViewModels;

namespace AppRpgEtect;

public partial class AppShell : Shell
{
    AppShellViewModel viewModel;

    public AppShell()
    {
        InitializeComponent();
        viewModel= new AppShellViewModel();
        BindingContext = new Binding();    
      
		
		string login = Preferences.Get("UsuarioUsername", string.Empty);
        lblLogin.Text= $"Login: {login}";
          
    }
}
