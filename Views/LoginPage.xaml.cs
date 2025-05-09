using PsicoAgenda.Services;

namespace PsicoAgenda.Views;

public partial class LoginPage : ContentPage
{
	private readonly ApiService _apiService;
	private readonly AuthService _authService;

	public LoginPage()
	{
		InitializeComponent();
		_apiService = new ApiService();
		_authService = new AuthService();
	}

	private async void OnLoginClicked(object sender, EventArgs e)
	{
		var email = EmailEntry.Text;
		var password = PasswordEntry.Text;

		try
		{
			var response = await _apiService.PostAsync<LoginResponse>("Cuentas/login", new
			{
				Email = email,
				Password = password
			});

			await _authService.SaveTokenAsync(response.Token);
			await Shell.Current.GoToAsync("//Inicio");
		}
		catch (Exception _)
		{
			await DisplayAlert("Error", "Credenciales incorrectos", "OK");
		}
	}

	private void OnRegisterClicked(object sender, EventArgs e)
	{
		Shell.Current.GoToAsync("//RegisterPage");
	}
}

public class LoginResponse
{
	public string Token { get; set; }
}