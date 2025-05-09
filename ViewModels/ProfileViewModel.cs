using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PsicoAgenda.Dtos;
using PsicoAgenda.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PsicoAgenda.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly AuthService _authService;
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string nombre;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string telefono;

        [ObservableProperty]
        private string especialidad;

        [ObservableProperty]
        private string direccion;

        [ObservableProperty]
        private string horarioAtencion;

        public ProfileViewModel()
        {
            _authService = new AuthService();
            _apiService = new ApiService();
            LoadUserDataCommand = new AsyncRelayCommand(LoadUserDataAsync);
            LogoutCommand = new AsyncRelayCommand(LogoutAsync);
        }

        public IAsyncRelayCommand LoadUserDataCommand { get; }
        public IAsyncRelayCommand LogoutCommand { get; }

        private async Task LoadUserDataAsync()
        {
            var token = await _authService.GetTokenAsync();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Obtener datos básicos del token
            email = jwtToken.Claims.First(c => c.Type == ClaimTypes.Email).Value;
            nombre = jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value;

            // Obtener datos adicionales del psicólogo desde la API
            var userId = jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var psicologo = await _apiService.GetAsync<PsicologoDto>($"Psicologos/{userId}");

            telefono = psicologo.Telefono;
            especialidad = psicologo.Especialidad;
            direccion = psicologo.Direccion;
            horarioAtencion = psicologo.HorarioAtencion;
        }

        private async Task LogoutAsync()
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Cerrar Sesión",
                "¿Estás seguro de que quieres salir?",
                "Sí", "No");

            if (confirm)
            {
                await _authService.LogoutAsync();
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}
