namespace Companion.ViewModels
{
    public partial class EvenementViewModel : BaseViewModel
    {
        HttpClient httpClient;

        [ObservableProperty]
        private ObservableCollection<Evenement> evenementen = default!;

        [ObservableProperty]
        private string naam;

        [ObservableProperty]
        private DateTime startDatum;

        [ObservableProperty]
        private DateTime eindDatum;

        [ObservableProperty]
        private decimal prijs;

        [ObservableProperty]
        private string opmerking;

        [ObservableProperty]
        private Product product;

        public EvenementViewModel()
        {
            Title = "Evenementen";
            httpClient = CreateHttpClientWithNoSSLValidation();
            Evenementen = new ObservableCollection<Evenement>();
            ToonEvenementen();
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
        public async Task ToonEvenementen()
        {
            var apiUrl = $"https://192.168.0.201:7153/Evenement";

            var response = await httpClient.GetStringAsync(apiUrl);
            var evenementenLijst = JsonSerializer.Deserialize<List<Evenement>>(response);

            foreach (var item in evenementenLijst)
            {
                Evenementen.Add(item);
            }
        }
    }
}
