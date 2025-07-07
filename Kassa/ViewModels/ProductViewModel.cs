namespace Kassa.ViewModels
{
    public partial class ProductViewModel : BaseViewModel
    {
        private byte[] imageData;

        [ObservableProperty]
        public ObservableCollection<Product> products = default!;

        [ObservableProperty]
        private ObservableCollection<Producttype> producttypes;

        [ObservableProperty]
        public Product selectedProduct;

        [ObservableProperty]
        private string validatieText = default!;

        private IProductRepository _productRepository;
        private IProducttypeRepository _producttypeRepository;

        public ProductViewModel(ProductRepository productRepository, ProducttypeRepository producttypeRepository)
        {
            _productRepository = productRepository;
            _producttypeRepository = producttypeRepository;
            Producttypes = new ObservableCollection<Producttype>(_producttypeRepository.GetProducttypes());
            RefreshProducts();
            selectedProduct = new Product();
        }

        [ObservableProperty]
        public string actieLabel = "Nieuw product toevoegen";

        partial void OnSelectedProductChanged(Product value)
        {
            if (value.Id == 0 || value == null)
            {
                ActieLabel = "Nieuw product toevoegen";
            }
            else
            {
                ActieLabel = "Product aanpassen";
            }
        }

        private void RefreshProducts()
        {
            Products = new ObservableCollection<Product>(_productRepository.GetProducts());
        }

        [RelayCommand]
        public void DeselectProduct()
        {
            ValidatieText = "";
            SelectedProduct = new Product();
        }

        [RelayCommand]
        public void AddProduct()
        {
            if (!IsModelValid()) { return; }
            if (SelectedProduct == null || SelectedProduct.Id != 0)
            {
                // Toon een foutmelding als SelectedProduct niet null is en de Id niet gelijk is aan 0
                Shell.Current.DisplayAlert("Fout", "Kan geen nieuw product toevoegen omdat er al een product is geselecteerd", "OK");
                return;
            }

            var result = _productRepository.CreateProduct(SelectedProduct);

            if (result)
            {
                RefreshProducts();
                SelectedProduct = new Product();
            }
            else
            {
                Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het toevoegen van het product", "OK");
            }
        }

        [RelayCommand]
        public void UpdateProduct()
        {
            if (!IsModelValid()) { return; }

            var result = _productRepository.UpdateProduct(SelectedProduct);

            if (result)
            {
                RefreshProducts();
                SelectedProduct = new Product();
            }
            else
            {
                Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het aanpassen van het product", "OK");
            }
        }

        [RelayCommand]
        public void DeleteProduct()
        {
            var result = _productRepository.DeleteProduct(SelectedProduct.Id);

            if (result)
            {
                RefreshProducts();
                SelectedProduct = new Product();
            }
            else
            {
                Shell.Current.DisplayAlert("Fout", "Er is een fout opgetreden bij het verwijderen van het product", "OK");
            }
        }

        [RelayCommand]
        public async Task UploadAfbeelding()
        {
            var result = await FilePicker.Default.PickAsync();

            if (result != null)
            {
                try
                {
                    var imageBytes = await File.ReadAllBytesAsync(result.FullPath);
                    SelectedProduct.Afbeelding = imageBytes;
                    OnPropertyChanged(nameof(SelectedProduct));
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Fout", $"Kan bestand niet lezen: {ex.Message}", "OK");
                }
            }
        }

        public bool IsModelValid()
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrWhiteSpace(SelectedProduct.Naam))
            {
                validationErrors.Add("Vul een naam in aub");
            }
            if (SelectedProduct.Prijs == 0)
            {
                validationErrors.Add("Vul een prijs in aub");
            }
            if (SelectedProduct.Aantal == 0)
            {
                validationErrors.Add("Vul het aantal in aub");
            }
            if (SelectedProduct.Afbeelding == null)
            {
                validationErrors.Add("Geef een afbeelding mee aub");
            }
            if (Products.Any(p => p.Naam.Equals(SelectedProduct.Naam, StringComparison.OrdinalIgnoreCase) &&
                                  p.Id != SelectedProduct.Id))
            {
                validationErrors.Add("Een product met dezelfde naam en beschrijving bestaat al");
            }
            if (validationErrors.Any())
            {
                ValidatieText = string.Join("\n", validationErrors);
                return false;
            }
            else
            {
                ValidatieText = "";
                return true;
            }
        }

        [RelayCommand]
        public void FilterFrisdrank()
        {
            Products = new ObservableCollection<Product>(_productRepository.FilterProductByFrisdrank());
        }

        [RelayCommand]
        public void FilterSnack()
        {
            Products = new ObservableCollection<Product>(_productRepository.FilterProductBySnack());
        }

        [RelayCommand]
        public void FilterAlcoholischeDrank()
        {
            Products = new ObservableCollection<Product>(_productRepository.FilterProductByAlcoholisch());
        }

        [RelayCommand]
        public void FilterWarmeDrank()
        {
            Products = new ObservableCollection<Product>(_productRepository.FilterProductByWarmeDrank());
        }

        [RelayCommand]
        public void FilterGoedkoopNaarDuur()
        {
            Products = new ObservableCollection<Product>(_productRepository.FilterProductenPrijsOplopend());
        }

        [RelayCommand]
        public void FilterDuurNaarGoedkoop()
        {
            Products = new ObservableCollection<Product>(_productRepository.FilterProductenPrijsAflopend());
        }
    }
}
