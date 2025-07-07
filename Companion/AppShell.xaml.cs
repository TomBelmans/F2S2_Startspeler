using Companion.Views;

namespace Companion
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistreerPage), typeof(RegistreerPage));
            Routing.RegisterRoute(nameof(MenukaartPage), typeof(MenukaartPage));
            Routing.RegisterRoute(nameof(EvenementPage), typeof(EvenementPage));
            Routing.RegisterRoute(nameof(WinkelwagenPage), typeof(WinkelwagenPage));
        }
    }
}
