using PsicoAgenda.Services;
using PsicoAgenda.Dtos;
using PsicoAgenda.ViewModels;

namespace PsicoAgenda.Views;

public partial class HomePage : ContentPage
{
	private HomeViewModel _viewModel;
	public HomePage()
	{
		InitializeComponent();
		
		_viewModel = new HomeViewModel();
        BindingContext = _viewModel;

		_viewModel.LoadCitasCommand.Execute(null);
    }

}