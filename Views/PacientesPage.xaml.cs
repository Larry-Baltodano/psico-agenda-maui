namespace PsicoAgenda.Views;

public partial class PacientesPage : ContentPage
{
	public PacientesPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
    {
        base.OnAppearing();

		var vm = BindingContext as PsicoAgenda.ViewModels.PacientesViewModel;
		if (vm != null && vm.LoadPacientesCommand.CanExecute(null))
		{
			await vm.LoadPacientesCommand.ExecuteAsync(null);
        }
    }
}