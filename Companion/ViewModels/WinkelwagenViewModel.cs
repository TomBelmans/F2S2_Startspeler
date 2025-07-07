namespace Companion.ViewModels
{
    [QueryProperty(nameof(Winkelmand), "Winkelmand")]
    [QueryProperty(nameof(Gebruiker), "Gebruiker")]
    [QueryProperty(nameof(TotaalPrijsWinkelmand), "TotaalPrijsWinkelmand")]
    public partial class WinkelwagenViewModel : BaseViewModel
    {
        HttpClient httpClient;

        [ObservableProperty]
        public ObservableCollection<Orderlijn> winkelmand;

        [ObservableProperty]
        public Bestelling bestelling;

        [ObservableProperty]
        public string geselecteerdeTafelNummer;

        [ObservableProperty]
        public string opmerking;

        [ObservableProperty]
        public string klantNaam;

        [ObservableProperty]
        public Gebruiker gebruiker;

        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private decimal totaalPrijsWinkelmand;


        public WinkelwagenViewModel()
        {
            Title = "Winkelwagen";
            httpClient = CreateHttpClientWithNoSSLValidation();
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


        [RelayCommand]
        public void VerwijderProduct(Orderlijn orderlijn)
        {
            Winkelmand.Remove(orderlijn);
            TotaalPrijsWinkelmand = Winkelmand.Sum(orderlijn => orderlijn.Product.prijs * orderlijn.TotaalAantal);
        }

        [RelayCommand]
        public async Task PlaatsBestelling()
        {
            if (Winkelmand == null || Winkelmand.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Uw winkelwagen is leeg.", "OK");
                return;
            }

            if (Gebruiker == null)
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Gebruikersgegevens ontbreken.", "OK");
                return;
            }

            bestelling = new Bestelling
            {
                GebruikerId = Gebruiker.id,
                KlantNaam = KlantNaam,
                TafelNummer = GeselecteerdeTafelNummer,
                TotaalPrijs = 0, // We berekenen de totale prijs na het toevoegen van producten
                Datum = DateTime.Now,
                IsBetaald = false,
                Opmerking = Opmerking,
                Orderlijnen = new List<Orderlijn>()
            };

            var json = JsonSerializer.Serialize(bestelling);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://192.168.0.201:7153/Bestelling", content);
            if (response.IsSuccessStatusCode)
            {
                // Parse the response content to get the orderId
                var responseContent = await response.Content.ReadAsStringAsync();

                // Attempt to parse the response content directly as an integer
                if (int.TryParse(responseContent, out var orderId))
                {
                    Console.WriteLine($"Order ID: {orderId}");

                    // Add products to the order using the retrieved order ID
                    await VoegProductenAanBestellingToe(orderId);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Fout", "Ongeldige bestelling ID ontvangen.", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Fout", "Fout bij het plaatsen van uw bestelling.", "OK");
            }


        }

        private async Task VoegProductenAanBestellingToe(int bestellingId)
        {
            bool success = true;
            foreach (var orderlijn in Winkelmand)
            {
                orderlijn.BestellingId = bestellingId;
                var json = JsonSerializer.Serialize(orderlijn);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("https://192.168.0.201:7153/Bestelling/VoegProductAanBestellingToe", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    await Application.Current.MainPage.DisplayAlert("Fout", $"Er is iets misgegaan bij het toevoegen van product {orderlijn.Product.naam}: {errorMessage}", "OK");
                    success = false;
                    break;
                }
            }

            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Succes", "Uw bestelling is geplaatst.", "OK");
                Winkelmand.Clear();
                // Clear the input fields
                GeselecteerdeTafelNummer = string.Empty;
                Opmerking = string.Empty;
                KlantNaam = string.Empty;
                TotaalPrijsWinkelmand = 0;
            }

            await Shell.Current.GoToAsync($"//{nameof(MenukaartPage)}");
        }
    }
}
