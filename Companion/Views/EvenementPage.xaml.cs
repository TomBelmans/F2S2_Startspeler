namespace Companion.Views;

public partial class EvenementPage : ContentPage
{
    public EvenementPage(EvenementViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}