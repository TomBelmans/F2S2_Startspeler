namespace Kassa.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        [ObservableProperty]
        public ObservableCollection<Gebruiker> gebruikers = default!;

        [ObservableProperty]
        public string email;

        [ObservableProperty]
        public string password;

        [ObservableProperty]
        public Gebruiker gebruiker;

        private IGebruikerRepository _gebruikerRepository;
        private IPasswordHasher<Gebruiker> _passwordHasher;

        public LoginViewModel(GebruikerRepository gebruikerRepository, IPasswordHasher<Gebruiker> passwordHasher)
        {
            _gebruikerRepository = gebruikerRepository;
            _passwordHasher = passwordHasher;
        }

        public void MaakVeldenLeeg()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        // Inloggen
        [RelayCommand]
        public async Task Login()
        {
            if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password))
            {
                Gebruiker gebruiker = _gebruikerRepository.GebruikerLogin(Email);

                if (gebruiker != null && gebruiker.Rol.Name != "Klant")
                {
                    var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(gebruiker, gebruiker.PasswordHash, Password);
                    if (passwordVerificationResult == PasswordVerificationResult.Success)
                    {
                        Preferences.Set("Rol", gebruiker.Rol.Name);
                        Preferences.Set("VolledigeNaam", gebruiker.VolledigeNaam);
                        await Shell.Current.GoToAsync("//ProductenPage");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Oeps...", "Wachtwoord onjuist.", "OK");
                    }
                }
                else if (gebruiker != null && gebruiker.Rol.Name == "Klant")
                {
                    await Shell.Current.DisplayAlert("Melding", "Een klant is niet bevoegd om in te loggen.", "OK");
                    Email = String.Empty;
                    Password = String.Empty;
                }
                else
                {
                    await Shell.Current.DisplayAlert("Oeps...", "De gebruiker met dit email adres is niet gevonden.", "OK");
                }
            }
        }

        // Uitloggen
        [RelayCommand]
        public async Task Logout()
        {
            Preferences.Clear();

            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");

            Email = string.Empty;
            Password = string.Empty;
        }
    }
}
