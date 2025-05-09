using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PsicoAgenda.Services;
using PsicoAgenda.Dtos;

namespace PsicoAgenda.ViewModels
{
    public partial class PacienteModalViewModel : ObservableObject
    {
        private readonly ApiService _apiService;

        [ObservableProperty]
        private string title = "Nuevo Paciente";

        [ObservableProperty]
        private string saveButtonText = "Guardar";

        [ObservableProperty]
        private bool isEditing = false;

        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string nombre;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string telefono;

        [ObservableProperty]
        private string direccion;

        [ObservableProperty]
        private string nota;

        [ObservableProperty]
        private int psicologoId;

        public PacienteModalViewModel()
        {
            _apiService = new ApiService();
            SaveCommand = new AsyncRelayCommand(SavePacienteAsync);
            CancelCommand = new AsyncRelayCommand(CancelAsync);
        }

        public IAsyncRelayCommand SaveCommand { get; }
        public IAsyncRelayCommand CancelCommand { get; }

        public void LoadPacienteData(PacienteDto paciente)
        {
            isEditing = true;
            title = "Editar Paciente";
            saveButtonText = "Actualizar";
            id = paciente.Id;
            nombre = paciente.Nombre;
            email = paciente.Email;
            telefono = paciente.Telefono;
            direccion = paciente.Direccion;
            nota = paciente.Nota;
            psicologoId = paciente.PsicologoId;
        }

        private async Task SavePacienteAsync()
        {
            var pacienteData = new PacienteDto
            {
                Nombre = this.nombre,
                Email = this.email,
                Telefono = this.telefono,
                Direccion = this.direccion,
                Nota = this.nota,
                PsicologoId = this.psicologoId
            };

            if (isEditing)
            {
                await _apiService.PutAsync<object>($"Pacientes/{id}", pacienteData);
            }
            else
            {
                await _apiService.PostAsync<object>("Pacientes", pacienteData);
            }

            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task CancelAsync()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }
    }
}
