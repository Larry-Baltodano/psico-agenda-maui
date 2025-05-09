using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PsicoAgenda.Dtos;
using PsicoAgenda.Services;

namespace PsicoAgenda.ViewModels
{
    public partial class CitaModalViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string title = "Nueva cita";

        [ObservableProperty]
        private string saveButtonText = "Guardar";

        [ObservableProperty]
        private bool isEditing = false;

        [ObservableProperty]
        private int citaId;

        [ObservableProperty]
        private int pacienteId;

        [ObservableProperty]
        private int psicologoId;

        [ObservableProperty]
        private int duracionMinutos = 30;

        [ObservableProperty]
        private DateTime fechaCita = DateTime.Today;

        [ObservableProperty]
        private TimeSpan horaCita = new(14, 0, 0);

        [ObservableProperty]
        private string estado = "Programada";

        [ObservableProperty]
        private List<string> estados = new List<string> { "Programada", "Cancelada", "Completada" };

        public CitaModalViewModel()
        {
            _apiService = new ApiService();
            SaveCommand = new AsyncRelayCommand(SaveCitaAsync);
            CancelCommand = new AsyncRelayCommand(CancelAsync);
        }

        public IAsyncRelayCommand SaveCommand { get; }
        public IAsyncRelayCommand CancelCommand { get; }

        public void LoadCitaData(CitaDto cita)
        {
            isEditing = true;
            title = "Editar Cita";
            saveButtonText = "Actalizar";
            citaId = cita.Id;
            pacienteId = cita.PacienteId;
            psicologoId = cita.PsicologoId;
            duracionMinutos = cita.DuracionMinutos;
            fechaCita = cita.FechaCita.Date;
            horaCita = cita.FechaCita.TimeOfDay;
            estado = cita.Estado;
        }

        private async Task SaveCitaAsync()
        {
            var fechaCompleta = fechaCita.Add(horaCita);

            if (isEditing)
            {
                await _apiService.PutAsync<Object>($"Citas/{citaId}", new
                {
                    pacienteId,
                    psicologoId,
                    duracionMinutos,
                    fechaCita = fechaCompleta,
                    estado
                });

                await Shell.Current.Navigation.PopModalAsync();
            }
            else
            {
                var nuevaCita = new
                {
                    pacienteId,
                    psicologoId,
                    duracionMinutos,
                    fechaCita = fechaCompleta,
                    estado
                };

                await _apiService.PostAsync<object>("Citas", nuevaCita);
                await Shell.Current.Navigation.PopModalAsync();
            }
        }

        private async Task CancelAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
