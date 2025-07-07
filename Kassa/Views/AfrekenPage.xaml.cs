namespace Kassa.Views;

public partial class AfrekenPage : ContentPage
{
    public AfrekenPage(AfrekenViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Title = "Welkom " + Preferences.Get("VolledigeNaam", "") + " - Bestellingen afrekenen";
    }
}