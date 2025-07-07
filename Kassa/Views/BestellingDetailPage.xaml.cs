namespace Kassa.Views;

public partial class BestellingDetailPage : ContentPage
{
    public BestellingDetailPage(BestellingDetailViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Title = "Welkom " + Preferences.Get("VolledigeNaam", "") + " detail van bestelling ";
    }
}