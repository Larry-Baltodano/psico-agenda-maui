using PsicoAgenda.ViewModels;
namespace PsicoAgenda.Views;

public partial class CitasPage : ContentPage
{
	public CitasPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var vm = BindingContext as CitasViewModel;
        if (vm != null && vm.LoadCitasCommand.CanExecute(null))
        {            
            await vm.LoadCitasCommand.ExecuteAsync(null);
        }
    }
}