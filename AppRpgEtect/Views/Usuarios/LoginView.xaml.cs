using AppRpgEtect.ViewModels.Usuarios;

namespace AppRpgEtect.Views.Usuarios;

public partial class LoginView : ContentPage
{
	UsuarioViewModel usuarioViewModel;

	public LoginView() 
	{
		InitializeComponent();

		usuarioViewModel = new UsuarioViewModel();
		BindingContext = usuarioViewModel;
	}
}