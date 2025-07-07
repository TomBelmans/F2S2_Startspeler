namespace Companion.Views;

public partial class LoginPage : ContentPage
{
    private LoginViewModel _viewModel;
    public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.MaakVeldenLeeg();
    }
}