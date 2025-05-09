using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PsicoAgenda.Dtos;
using PsicoAgenda.Views;
using PsicoAgenda.Services;
using System.Collections.ObjectModel;

namespace PsicoAgenda.ViewModels
{
    public partial class PacientesViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<PacienteDto> pacientes = new();

        [ObservableProperty]
        private string searchTerm;

        public PacientesViewModel()
        {
            _apiService = new ApiService();
            LoadPacientesCommand = new AsyncRelayCommand(LoadPacientesAsync);
            SearchCommand = new AsyncRelayCommand(SearchPacientesAsync);
            ShowModalCommand = new AsyncRelayCommand(ShowModalAsync);
            EditCommand = new AsyncRelayCommand<PacienteDto>(EditPacienteAsync);
            DeleteCommand = new AsyncRelayCommand<PacienteDto>(DeletePacienteAsync);
        }

        public IAsyncRelayCommand LoadPacientesCommand { get; }
        public IAsyncRelayCommand SearchCommand { get; }
        public IAsyncRelayCommand ShowModalCommand { get; }
        public IAsyncRelayCommand<PacienteDto> EditCommand { get; }
        public IAsyncRelayCommand<PacienteDto> DeleteCommand { get; }

        private async Task LoadPacientesAsync()
        {
            var pacientes = await _apiService.GetAsync<List<PacienteDto>>("Pacientes");
            Pacientes = new ObservableCollection<PacienteDto>(pacientes);
        }

        private async Task SearchPacientesAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                await LoadPacientesAsync();
                return;
            }

            var resultados = Pacientes
                .Where(p => p.Nombre.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                           p.Id.ToString().Contains(SearchTerm))
                .ToList();

            Pacientes = new ObservableCollection<PacienteDto>(resultados);
        }

        private async Task ShowModalAsync()
        {
            var vm = new PacienteModalViewModel();
            var modal = new PacientesModalPage
            {
                BindingContext = vm
            };

            await Shell.Current.Navigation.PushModalAsync(modal);
        }

        private async Task EditPacienteAsync(PacienteDto paciente)
        {
            var vm = new PacienteModalViewModel();
            vm.LoadPacienteData(paciente);

            var modal = new PacientesModalPage
            {
                BindingContext = vm
            };

            await Shell.Current.Navigation.PushModalAsync(modal);
        }

        private async Task DeletePacienteAsync(PacienteDto paciente)
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Confirmar",
                $"¿Eliminar a {paciente.Nombre}?",
                "Sí", "No");

            if (confirm)
            {
                await _apiService.DeleteAsync($"Pacientes/{paciente.Id}");
                await LoadPacientesAsync();
            }
        }
    }
}
