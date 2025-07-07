namespace Companion.Views;

public partial class WinkelwagenPage : ContentPage
{
    public WinkelwagenPage(WinkelwagenViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
