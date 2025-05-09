using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using PsicoAgenda.Services;
using PsicoAgenda.Views;
using System.Windows.Input;

namespace PsicoAgenda.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private readonly AuthService _authService;

        [ObservableProperty]
        private string nombreCompleto;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string telefono;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private string rol;
        public List<string> Roles { get; } = new List<string> { "Psicologo", "Admin"};

        [ObservableProperty]
        private string especialidad;

        [ObservableProperty]
        private string direccion;

        [ObservableProperty]
        private string horarioAtencion;

        public ICommand RegisterCommand { get; }
        public ICommand GoToLoginCommand { get; }

        public RegisterViewModel()
        {
            _apiService = new ApiService();
            _authService = new AuthService();

            RegisterCommand = new RelayCommand(OnRegister);
            GoToLoginCommand = new RelayCommand(GoToLogin);
        }

        private async void OnRegister()
        {
            if (password != confirmPassword)
            {
                await Shell.Current.DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
                return;
            }

            try
            {
                var resgitroData = new
                {
                    NombreCompleto = this.nombreCompleto,
                    Email = this.email,
                    Password = this.password,
                    Rol = this.rol,
                    Telefono = this.telefono,
                    Especialidad = this.especialidad,
                    Direccion = this.direccion,
                    HorarioAtencion = this.horarioAtencion
                };

                var response = await _apiService.PostAsync<LoginResponse>("Cuentas/registrar", resgitroData);

                await _authService.SaveTokenAsync(response.Token);
                await Shell.Current.GoToAsync("//HomePage");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo completar el registro", "OK");
            }
        }

        private void GoToLogin()
        {
            Shell.Current.GoToAsync("//LoginPage");
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
