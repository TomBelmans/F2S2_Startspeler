namespace Companion.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        HttpClient httpClient;

        [ObservableProperty]
        public ObservableCollection<Gebruiker> gebruikers = default!;

        [ObservableProperty]
        public string voornaam;

        [ObservableProperty]
        public string achternaam;

        [ObservableProperty]
        public string gebruikersnaam;

        [ObservableProperty]
        public string email;

        [ObservableProperty]
        public string wachtwoord;

        [ObservableProperty]
        private string bevestigWachtwoord;

        [ObservableProperty]
        public Gebruiker gebruiker;

        // Regex
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex WachtwoordRegex = new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[\W_]).{6,}$", RegexOptions.Compiled);

        public LoginViewModel()
        {
            httpClient = CreateHttpClientWithNoSSLValidation();
            Gebruiker = new Gebruiker();
        }

        // Schakel de SSL validatie uit om in te loggen op de Android Emulator
        private HttpClient CreateHttpClientWithNoSSLValidation()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromSeconds(200);
            return client;
        }

        // Inloggen
        [RelayCommand]
        public async Task Login()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Wachtwoord))
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Email en wachtwoord mogen niet leeg zijn.", "Ok");
                return;
            }

            var apiUrl = $"https://192.168.0.201:7153/Account/login?email={Email}&password={Wachtwoord}";
            var response = await httpClient.PostAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var ingelogdeGebruiker = JsonSerializer.Deserialize<Gebruiker>(jsonResponse);

                await Shell.Current.GoToAsync("//MenukaartPage");
                Preferences.Set("GebruikerId", ingelogdeGebruiker.id);

            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                await Application.Current.MainPage.DisplayAlert("Fout", "Login mislukt: " + errorMessage, "Ok");
            }
        }

        public void MaakVeldenLeeg()
        {
            Email = string.Empty;
            Wachtwoord = string.Empty;
        }

        // Uitloggen
        [RelayCommand]
        public async Task Logout()
        {
            var apiUrl = "https://192.168.0.201:7153/Account/logout";
            var response = await httpClient.PostAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                Preferences.Remove("GebruikerId");

                SecureStorage.RemoveAll();
                if (Gebruikers != null)
                {
                    Gebruikers.Clear();
                }
                Preferences.Clear();

                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Uitloggen mislukt.", "Ok");
            }
        }

        // Navigeer naar registratiepagina
        [RelayCommand]
        public void NavigeerNaarRegistratie()
        {
            Shell.Current.GoToAsync(nameof(RegistreerPage), true);
        }

        // Nieuwe gebruiker aanmaken
        [RelayCommand]
        public async Task NieuweGebruiker()
        {
            if (string.IsNullOrEmpty(Gebruikersnaam) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Wachtwoord) || string.IsNullOrEmpty(BevestigWachtwoord) || string.IsNullOrEmpty(Voornaam) || string.IsNullOrEmpty(Achternaam))
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Alle velden moeten worden ingevuld.", "OK");
                return;
            }

            if (!EmailRegex.IsMatch(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Voer een geldig e-mailadres in.", "OK");
                return;
            }

            if (!WachtwoordRegex.IsMatch(Wachtwoord))
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Het wachtwoord voldoet niet aan de veiligheidseisen.", "OK");
                return;
            }

            if (Wachtwoord != BevestigWachtwoord)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "De wachtwoorden komen niet overeen.", "OK");
                return;
            }

            if (Wachtwoord.Length < 8)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Het wachtwoord moet minstens 8 karakters lang zijn.", "OK");
                return;
            }

            var apiUrl = $"https://192.168.0.201:7153/Account/register?username={Gebruikersnaam}&email={Email}&password={Wachtwoord}&voornaam={Voornaam}&achternaam={Achternaam}";
            var response = await httpClient.PostAsync(apiUrl, null);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var ingelogdeGebruiker = JsonSerializer.Deserialize<Gebruiker>(jsonResponse);

                await Shell.Current.GoToAsync("//MenukaartPage");
                Preferences.Set("GebruikerId", ingelogdeGebruiker.id);
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                await Application.Current.MainPage.DisplayAlert("Fout", "Registratie mislukt: " + errorMessage, "OK");
            }
        }
    }
}