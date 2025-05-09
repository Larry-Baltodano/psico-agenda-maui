using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PsicoAgenda.Dtos;
using PsicoAgenda.Views;
using PsicoAgenda.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PsicoAgenda.ViewModels
{
    public partial class CitasViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private ObservableCollection<CitaDto> citas = new();

        [ObservableProperty]
        private string searchTerm;      

        public CitasViewModel()
        {
            _apiService = new ApiService();
            LoadCitasCommand = new AsyncRelayCommand(LoadCitasAsync);
            SearchCommand = new AsyncRelayCommand(SearchCitasAsync);
            ShowModalCommand = new AsyncRelayCommand(ShowModalAsync);
            EditCommand = new AsyncRelayCommand<CitaDto>(EditCitaAsync);
            DeleteCommand = new AsyncRelayCommand<CitaDto>(DeleteCitaAsync);
        }

        public IAsyncRelayCommand LoadCitasCommand { get; }
        public IAsyncRelayCommand SearchCommand { get; }
        public IAsyncRelayCommand ShowModalCommand { get; }
        public IAsyncRelayCommand<CitaDto> EditCommand { get; }
        public IAsyncRelayCommand<CitaDto> DeleteCommand { get; }


        private async Task LoadCitasAsync()
        {
            try
            {
                var lista = await _apiService.GetAsync<List<CitaDto>>("Citas");

                citas.Clear();
                foreach (var cita in lista)
                {
                    cita.EstadoColor = GetEstadoColor(cita.Estado);
                    citas.Add(cita);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Error al cargar las citas: {ex.Message}", "OK");
            }
        }

        private async Task SearchCitasAsync()
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                await LoadCitasAsync();
                return;
            }

            var citasFiltradas = citas
                .Where(c => c.Id.ToString().Contains(searchTerm) ||
                c.PacienteNombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();

            citas.Clear();
            foreach (var cita in citasFiltradas)
            {
                citas.Add(cita);
            }
        }

        private async Task ShowModalAsync()
        {
            await Shell.Current.Navigation.PushModalAsync(new NuevaCitaPage());
        }
        
        private async Task EditCitaAsync(CitaDto cita)
        {
            if (cita == null) return;
            
            var vm = new CitaModalViewModel();
            vm.LoadCitaData(cita);

            var modal = new CitaModalPage
            {
                BindingContext = vm
            };

            modal.Disappearing += async (s, e) =>
            {
                await LoadCitasAsync();
            };           

            await Shell.Current.Navigation.PushModalAsync(modal);
        }

        private async Task DeleteCitaAsync(CitaDto cita)
        {
            if (cita == null) return;
            
            var confirm = await Shell.Current.DisplayAlert("Confirmar", "¿Desea eliminar esta cita?", "Si", "No");
            if (!confirm) return;            

            try
            {                
                await _apiService.DeleteAsync($"Citas/{cita.Id}");
                citas.Remove(cita);                
            }
            catch (Exception ex)
            {                
                await Shell.Current.DisplayAlert("Error", $"No se pudo eliminar la cita: {ex.Message}", "OK");
            }
        }

        private Color GetEstadoColor(string estado)
        {
            return estado switch
            {
                "Programada" => Colors.Blue,
                "Completada" => Colors.Green,
                "Cancelada" => Colors.Red,
                _ => Colors.Gray
            };
        }
    }
}
