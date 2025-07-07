namespace Companion.ViewModels
{
    public partial class MenukaartViewModel : BaseViewModel
    {
        HttpClient httpClient;

        [ObservableProperty]
        private ObservableCollection<Product> producten = default!;

        [ObservableProperty]
        private ObservableCollection<Orderlijn> winkelmand;

        [ObservableProperty]
        public Product geselecteerdProduct;

        [ObservableProperty]
        private ImageSource afbeeldingSource;

        [ObservableProperty]
        private string naam;

        [ObservableProperty]
        private decimal prijs;

        [ObservableProperty]
        private string beschrijving;

        [ObservableProperty]
        private int beschikbaarheid;

        [ObservableProperty]
        private decimal totaalPrijsWinkelmand;

        public MenukaartViewModel()
        {
            Title = "Menukaart";
            httpClient = CreateHttpClientWithNoSSLValidation();
            Producten = new ObservableCollection<Product>();
            ToonProducten();
            GeselecteerdProduct = new Product();
            Winkelmand = new ObservableCollection<Orderlijn>();
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
        public async Task ToonProducten()
        {
            var apiUrl = $"https://192.168.0.201:7153/Producten";

            var response = await httpClient.GetStringAsync(apiUrl);
            var productenLijst = JsonSerializer.Deserialize<List<Product>>(response);

            foreach (var item in productenLijst)
            {
                Producten.Add(item);
            }
        }

        public void VoegToeAanWinkelmand(Product product)
        {
            if (product != null)
            {
                var bestaandeOrderlijn = Winkelmand.FirstOrDefault(ol => ol.ProductId == product.id);

                if (product.AantalInWinkelmand > 0)
                {
                    if (bestaandeOrderlijn != null)
                    {
                        bestaandeOrderlijn.TotaalAantal = product.AantalInWinkelmand;
                    }
                    else
                    {
                        var nieuweOrderlijn = new Orderlijn
                        {
                            ProductId = product.id,
                            Product = product,
                            TotaalAantal = product.AantalInWinkelmand
                        };
                        Winkelmand.Add(nieuweOrderlijn);
                    }
                }
                else
                {
                    if (bestaandeOrderlijn != null)
                    {
                        Winkelmand.Remove(bestaandeOrderlijn);
                    }
                }

                // Notify the UI that the Winkelmand collection has changed
                OnPropertyChanged(nameof(Winkelmand));
                BerekenTotaalPrijs();
            }
        }

        public void BerekenTotaalPrijs()
        {
            TotaalPrijsWinkelmand = Winkelmand.Sum(ol => ol.TotaalAantal * ol.Product.prijs);
        }

        [RelayCommand]
        public async Task NavigateToWinkelwagen()
        {
            try
            {
                // Haal de ingelogde gebruiker op uit Preferences
                var gebruikerId = Preferences.Get("GebruikerId", string.Empty);
                if (string.IsNullOrEmpty(gebruikerId))
                {
                    await Application.Current.MainPage.DisplayAlert("Fout", "Gebruikersgegevens ontbreken.", "OK");
                    return;
                }

                var huidigeGebruiker = new Gebruiker { id = gebruikerId };


                var parameters = new Dictionary<string, object>
                    {
                        { "Winkelmand", Winkelmand },
                        { "Gebruiker", huidigeGebruiker },
                        { "TotaalPrijsWinkelmand", TotaalPrijsWinkelmand }
                    };

                await Shell.Current.GoToAsync(nameof(WinkelwagenPage), parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void MaakMenukaartLeeg()
        {
            // Zorg ervoor dat de winkelmand wordt gewist en de UI wordt bijgewerkt
            Winkelmand.Clear();
        }

        public void ResetSteppers()
        {
            foreach (var product in Producten)
            {
                product.AantalInWinkelmand = 0;
            }
        }
    }
}