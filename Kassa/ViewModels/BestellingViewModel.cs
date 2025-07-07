namespace Kassa.ViewModels
{
    public partial class BestellingViewModel : BaseViewModel
    {

        [ObservableProperty]
        public ObservableCollection<Bestelling> bestellingen;

        private Bestelling _bestellingen;

        [ObservableProperty]
        public Bestelling selectedBestelling;

        private Bestelling _selectedBestelling;

        private bool? _showVerwerkt = false; // Standaardwaarde niet betaalde bestellingen tonen

        public bool? ShowVerwerkt
        {
            get => _showVerwerkt;
            set
            {
                _showVerwerkt = value;
                OnPropertyChanged(nameof(ShowVerwerkt));
                RefreshBestellingen();
            }
        }


        private readonly IBestellingRepository _bestellingRepository;
        private readonly Gebruiker _huidigeGebruiker; // Gebruiker object om de huidige gebruiker op te slaan



        public BestellingViewModel(BestellingRepository bestellingRepository)
        {
            _bestellingRepository = bestellingRepository;
            RefreshBestellingen();


            MessagingCenter.Subscribe<BestellingDetailViewModel>(this, "RefreshBestellingen", async (sender) =>
            {
                await RefreshBestellingen();
            });

            Task.Run(PollForUpdates); // Start de polling loop
        }

        // Bestellingen refreshen met een polling loop om de 20 seconden
        private async Task PollForUpdates()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(20)); // Pollen elke 20 seconden
                await RefreshBestellingen();
            }
        }

        private async Task RefreshBestellingen()
        {
            var bestellingenMetProducten = await Task.Run(() => _bestellingRepository.GetBestellingenMetProducten());

            // Filter orders based on ShowBetaald
            var filteredBestellingen = bestellingenMetProducten
                .Where(b => _showVerwerkt == null || _showVerwerkt == b.BestellingVerwerkt)
                .ToList();

            Bestellingen = new ObservableCollection<Bestelling>(filteredBestellingen);
        }

        [RelayCommand]
        public async Task OpenDetailPage(Bestelling bestelling)
        {
            if (bestelling != null)
            {
                try
                {
                    await Shell.Current.GoToAsync($"{nameof(BestellingDetailPage)}", true,
                        new Dictionary<string, object>
                        {
                    { "BestellingId", bestelling.Id }
                        });
                }
                catch (Exception ex)
                {
                    // Log de fout voor debuggen
                    Console.WriteLine($"Navigatiefout: {ex.Message}");
                    // Eventueel een melding aan de gebruiker geven
                    await Application.Current.MainPage.DisplayAlert("Fout", "Kan de detailpagina niet openen.", "OK");
                }
            }
        }

        [RelayCommand]
        public async void ShowVerwerkteBestellingen()
        {
            ShowVerwerkt = true;
        }
        [RelayCommand]
        public async void ShowNietVerwerkteBestellingen()
        {
            ShowVerwerkt = false;
        }
        [RelayCommand]
        public async void ShowAlleBestellingen()
        {
            ShowVerwerkt = null; // Stel ShowBetaald in op true om alle bestellingen weer te geven
        }

        [RelayCommand]
        public async Task NavigeerNaarAfrekenen()
        {
            await Shell.Current.GoToAsync($"{nameof(AfrekenPage)}", true);
        }

        private byte[] ExcelExportService(List<Bestelling> bestellingen)
        {
            // EPPlus Licentie acceptatie
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Bestellingen");

                // Tabel hoofding
                worksheet.Cells[1, 1].Value = "Bestelling Id";
                worksheet.Cells[1, 2].Value = "Gebruiker Naam";
                worksheet.Cells[1, 3].Value = "Datum";
                worksheet.Cells[1, 4].Value = "Klantnaam";
                worksheet.Cells[1, 5].Value = "Tafelnummer";
                worksheet.Cells[1, 6].Value = "Totaalprijs";
                worksheet.Cells[1, 7].Value = "IsBetaald";
                worksheet.Cells[1, 8].Value = "Bestelde Producten";

                // Waarden per rij
                for (int i = 0; i < bestellingen.Count; i++)
                {
                    var bestelling = bestellingen[i];
                    worksheet.Cells[i + 2, 1].Value = bestelling.Id;
                    worksheet.Cells[i + 2, 2].Value = bestelling.Gebruiker.VolledigeNaam;
                    worksheet.Cells[i + 2, 3].Value = bestelling.Datum.ToString("dd-MM-yyyy");
                    worksheet.Cells[i + 2, 4].Value = bestelling.KlantNaam;
                    worksheet.Cells[i + 2, 5].Value = bestelling.TafelNummer;
                    worksheet.Cells[i + 2, 6].Value = bestelling.TotaalPrijs;
                    worksheet.Cells[i + 2, 7].Value = bestelling.IsBetaald ? "Ja" : "Nee";

                    // Voeg bestelde producten toe
                    var besteldeProducten = string.Join(", ", bestelling.Orderlijnen.Select(ol => $"{ol.Product.Naam} (x{ol.TotaalAantal})"));
                    worksheet.Cells[i + 2, 8].Value = besteldeProducten;
                }

                return package.GetAsByteArray();
            }
        }

        [RelayCommand]
        public async Task ExportBestellingenToExcel()
        {
            try
            {
                var excelData = await Task.Run(() => ExcelExportService(Bestellingen.ToList()));

                var filePath = Path.Combine(FileSystem.CacheDirectory, "Bestellingen.xlsx");
                File.WriteAllBytes(filePath, excelData);

                await Application.Current.MainPage.DisplayAlert("Succes", $"Bestellingen zijn geëxporteerd naar {filePath}", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het exporteren naar Excel: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Fout", "Er is een fout opgetreden bij het exporteren naar Excel.", "OK");
            }
        }
    }
}
