namespace Kassa.ViewModels
{
    [QueryProperty(nameof(BestellingId), "BestellingId")]
    public partial class BestellingDetailViewModel : BaseViewModel
    {
        [ObservableProperty]
        public Bestelling bestelling;

        private readonly IBestellingRepository _bestellingRepository;
        private readonly Gebruiker _huidigeGebruiker;

        private int _bestellingId;
        public int BestellingId
        {
            get => _bestellingId;
            set
            {
                _bestellingId = value;
                LoadBestelling();
            }
        }

        public BestellingDetailViewModel(IBestellingRepository bestellingRepository)
        {
            _bestellingRepository = bestellingRepository;
            Bestelling = new Bestelling();
        }

        public void LoadBestelling()
        {
            if (BestellingId != 0)
            {
                Bestelling = _bestellingRepository.GetBestellingById(BestellingId);
            }
        }

        [RelayCommand]
        public void BewerkBestelling()
        {
            var result = _bestellingRepository.UpdateBestelling(Bestelling);

            if (result)
            {
                LoadBestelling();
                Bestelling = new Bestelling();
                MessagingCenter.Send(this, "RefreshBestellingen");
                Shell.Current.Navigation.PopAsync();
            }
        }

        [RelayCommand]
        [Obsolete]
        public async Task VerwijderBestelling()
        {
            var confirmDelete = await Application.Current.MainPage.DisplayAlert("Verwijderen", "Weet je zeker dat je deze bestelling wilt verwijderen?", "Ja", "Nee");
            if (confirmDelete)
            {
                // Verwijder de bestelling
                _bestellingRepository.VerwijderBestelling(BestellingId);
                MessagingCenter.Send(this, "RefreshBestellingen");
                await Shell.Current.Navigation.PopAsync();
            }
        }
    }
}
