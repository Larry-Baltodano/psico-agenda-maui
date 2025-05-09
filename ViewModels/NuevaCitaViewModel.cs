using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PsicoAgenda.Services;

namespace PsicoAgenda.ViewModels
{
    public partial class NuevaCitaViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

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
        private List<string> estados = new List<string> { "Programada", "Cancelada", "Completada"};

        public NuevaCitaViewModel()
        {
            _apiService = new ApiService();
            SaveCommand = new AsyncRelayCommand(SaveCitaAsync);
            CancelCommand = new AsyncRelayCommand(CancelAsync);
        }

        public IAsyncRelayCommand SaveCommand { get; }
        public IAsyncRelayCommand CancelCommand { get; }

        private async Task SaveCitaAsync()
        {
            var nuevaCita = new
            {
                PacienteId = this.pacienteId,
                psicologoId = this.psicologoId,
                DuracionMinutos = this.duracionMinutos,
                FechaCita = this.fechaCita.Add(this.horaCita),
                Estado = this.estado
            };

            await _apiService.PostAsync<Object>("Citas", nuevaCita);
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task CancelAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
