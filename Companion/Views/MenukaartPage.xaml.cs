namespace Companion.Views
{
    public partial class MenukaartPage : ContentPage
    {
        private MenukaartViewModel _viewModel;
        public MenukaartPage(MenukaartViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = viewModel;
        }

        private void OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            // Haal de geselecteerde product op basis van de BindingContext van de Stepper
            var stepper = (Stepper)sender;
            var product = (Product)stepper.BindingContext;

            // Roep de ViewModel method aan om het product te selecteren en de winkelmand bij te werken
            _viewModel.VoegToeAanWinkelmand(product);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = (MenukaartViewModel)BindingContext;
            viewModel.MaakMenukaartLeeg();
            viewModel.ResetSteppers();
        }
    }
}
