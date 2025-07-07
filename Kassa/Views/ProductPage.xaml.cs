namespace Kassa.Views;

public partial class ProductPage : ContentPage
{
    public ProductPage(ProductViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Title = "Welkom " + Preferences.Get("VolledigeNaam", "") + " - Een overzicht van de beschikbare producten";
    }
}