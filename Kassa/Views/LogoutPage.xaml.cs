namespace Kassa.Views;

public partial class LogoutPage : ContentPage
{
    private readonly GebruikerRepository _gebruikerRepository;
    private readonly IPasswordHasher<Gebruiker> _passwordHasher;

    public LogoutPage(GebruikerRepository gebruikerRepository, IPasswordHasher<Gebruiker> passwordHasher)
    {
        InitializeComponent();
        _gebruikerRepository = gebruikerRepository;
        _passwordHasher = passwordHasher;
        BindingContext = new LoginViewModel(_gebruikerRepository, _passwordHasher);
    }

    private void OnLogout(object sender, EventArgs e)
    {
        Preferences.Clear();
        Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        Application.Current.MainPage = new AppShell();
    }
}