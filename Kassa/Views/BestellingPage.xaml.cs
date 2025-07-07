namespace Kassa.Views;

public partial class BestellingPage : ContentPage
{
    public BestellingPage(BestellingViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Title = "Welkom " + Preferences.Get("VolledigeNaam", "") + " - Een overzicht van de bestellingen";
    }
}