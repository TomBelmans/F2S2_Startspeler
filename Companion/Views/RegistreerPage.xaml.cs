namespace Companion.Views;

public partial class RegistreerPage : ContentPage
{
	public RegistreerPage(LoginViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}