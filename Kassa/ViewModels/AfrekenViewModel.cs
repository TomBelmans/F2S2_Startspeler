namespace Kassa.ViewModels
{
    public partial class AfrekenViewModel : BaseViewModel
    {
        [ObservableProperty]
        private ObservableCollection<Bestelling> bestellingen;

        [ObservableProperty]
        private Bestelling selectedBestelling;

        [ObservableProperty]
        private Gebruiker gebruiker;

        [ObservableProperty]
        private string achternaam;

        [ObservableProperty]
        private Orderlijn orderlijn;

        [ObservableProperty]
        private Product product;

        [ObservableProperty]
        private string klantNaam;

        private readonly IAfrekenRepository _afrekenRepository;

        public AfrekenViewModel(IAfrekenRepository afrekenRepository)
        {
            _afrekenRepository = afrekenRepository;
            RefreshAfrekeningen();
        }

        [RelayCommand]
        private void RefreshAfrekeningen()
        {
            Bestellingen = new ObservableCollection<Bestelling>(_afrekenRepository.GetBestellingen());
        }

        public decimal TotaalPrijs => Bestellingen?.Sum(b => b.TotaalPrijs) ?? 0;

        [RelayCommand]
        private void Deselecteren()
        {
            SelectedBestelling = null;
        }

        [RelayCommand]
        private void Afrekenen()
        {
            if (SelectedBestelling != null)
            {
                _afrekenRepository.UpdateBestellingStatus(SelectedBestelling.Id, true);
                RefreshAfrekeningen();
            }
        }

        [RelayCommand]
        private void FilterOpAchternaam()
        {
            Bestellingen = new ObservableCollection<Bestelling>(_afrekenRepository.GetBestellingenOpNaam(Achternaam));
        }
    }
}
