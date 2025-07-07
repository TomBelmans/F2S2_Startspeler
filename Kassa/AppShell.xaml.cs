namespace Kassa
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(BestellingPage), typeof(BestellingPage));
            Routing.RegisterRoute(nameof(ProductPage), typeof(ProductPage));
            Routing.RegisterRoute(nameof(GebruikerPage), typeof(GebruikerPage));
            Routing.RegisterRoute(nameof(BestellingDetailPage), typeof(BestellingDetailPage));
            Routing.RegisterRoute(nameof(AfrekenPage), typeof(AfrekenPage));
            Routing.RegisterRoute(nameof(LogoutPage), typeof(LogoutPage));
        }
    }
}
