using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PsicoAgenda.Dtos;
using PsicoAgenda.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace PsicoAgenda.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string fechaActual = DateTime.Now.ToString("D", CultureInfo.CurrentCulture);

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private ObservableCollection<CitaDto> citasDelDia = new();
        
        public HomeViewModel()
        {
            _apiService = new ApiService();
            LoadCitasCommand = new AsyncRelayCommand(LoadCitasDelDiaAsync);
            NotificarCitaCommand = new AsyncRelayCommand<int>(NotificarCitaAsync);
        }

        public IAsyncRelayCommand LoadCitasCommand { get; }
        public IAsyncRelayCommand<int> NotificarCitaCommand { get; }

        private async Task LoadCitasDelDiaAsync()
        {
            try
            {
                isBusy = true;
                var citas = await _apiService.GetAsync<List<CitaDto>>("Citas/hoy");
                citasDelDia.Clear();

                foreach (var cita in citas)
                {
                    cita.EstadoColor = cita.Estado switch
                    {
                        "Programada" => Colors.Blue,
                        "Completada" => Colors.Green,
                        "Cancelada" => Colors.Red,
                        _ => Colors.Gray
                    };
                    citasDelDia.Add(cita);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", "No se pudieron cargar las citas", "OK");
            }
            finally
            {
                isBusy = false;
            }
        }

        private async Task NotificarCitaAsync(int citaId)
        {
            try
            {
                var response = await _apiService.PostAsync<object>($"Citas/notificar/{citaId}", null);
                await Shell.Current.DisplayAlert("Exito", "Notificación enviada con éxito", "OK");
            } 
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Ocurrio un error al notificar la cita: {ex.Message}", "OK");
            }
        }
    }
}
