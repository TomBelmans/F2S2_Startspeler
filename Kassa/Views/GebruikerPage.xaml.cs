namespace Kassa.Views;

public partial class GebruikerPage : ContentPage
{
	public GebruikerPage(GebruikerViewModel viewModel)
    {
		InitializeComponent();
        BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Title = "Welkom " + Preferences.Get("VolledigeNaam", "") + " - Beheer de gebruikers van deze applicatie";
    }
}