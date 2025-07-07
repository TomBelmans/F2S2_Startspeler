namespace Kassa.ViewModels
{
    public partial class GebruikerViewModel : BaseViewModel
    {
        [ObservableProperty]
        public ObservableCollection<Gebruiker> gebruikers = default!;

        [ObservableProperty]
        private ObservableCollection<Aspnetroles> rollen;

        [ObservableProperty]
        public string userName;

        [ObservableProperty]
        public string voornaam;

        [ObservableProperty]
        public string achternaam;

        [ObservableProperty]
        public string email;

        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public Gebruiker selectedGebruiker;

        [ObservableProperty]
        public bool isBeheerder;

        private IGebruikerRepository _gebruikerRepository;

        public GebruikerViewModel(GebruikerRepository gebruikerRepository)
        {
            _gebruikerRepository = gebruikerRepository;
            SelectedGebruiker = new Gebruiker();
            LoadRollen();
            RefreshGebruikers();
        }

        public void LoadRollen()
        {
            var rollenList = _gebruikerRepository.GetAlleRollen();
            Rollen = new ObservableCollection<Aspnetroles>(rollenList);
        }

        [ObservableProperty]
        public string actieLabel = "Speler bewerken";

        partial void OnSelectedGebruikerChanged(Gebruiker gebruiker)
        {
            if (gebruiker != null)
            {
                ActieLabel = "Gegevens bewerken van " + gebruiker.VolledigeNaam;
                if (gebruiker.Rol != null)
                {
                    SelectedGebruiker.Rol = gebruiker.Rol;
                }
            }
            else
            {
                ActieLabel = "Selecteer een gebruiker";
            }
        }

        private async Task RefreshGebruikers()
        {
            if (Preferences.Get("Rol", "") == "Beheerder")
            {
                IsBeheerder = true;
                Gebruikers = new ObservableCollection<Gebruiker>(_gebruikerRepository.GetGebruikers());
            }
            else
            {
                IsBeheerder = false;
                await Shell.Current.DisplayAlert("Melding", "U bent niet bevoegd om deze pagina te bekijken.", "OK");
            }
        }

        [RelayCommand]
        public async Task VerwijderenAsync()
        {
            var result = _gebruikerRepository.VerwijderenGebruiker(SelectedGebruiker.Id);

            if (result)
            {
                await RefreshGebruikers();
                SelectedGebruiker = new Gebruiker();
            }
            else
            {
                await Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het verwijderen van de gebruiker", "OK");
            }
        }

        [RelayCommand]
        public async Task WijzigenAsync()
        {
            var result = _gebruikerRepository.WijzigenGebruiker(SelectedGebruiker);
            if (result)
            {
                await RefreshGebruikers();
                SelectedGebruiker = new Gebruiker();
            }
            else
            {
                await Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het wijzigen van de gebruiker", "OK");
            }
        }

        [RelayCommand]
        public void Deselecteren()
        {
            SelectedGebruiker = new Gebruiker();
        }
    }
}
