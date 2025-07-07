using Microsoft.Extensions.Logging;

namespace Kassa
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // Deze service hashed het wachtwoord dat ingegeven wordt op de inlogpagina. Dit wordt vergeleken met het gehashte wachtwoord in de databank.
            builder.Services.AddScoped<IPasswordHasher<Gebruiker>, PasswordHasher<Gebruiker>>();
            builder.Services.AddSingleton<IBestellingRepository, BestellingRepository>();

            // Repositories
            builder.Services.AddSingleton<BestellingRepository>();
            builder.Services.AddSingleton<IAfrekenRepository, AfrekenRepository>();
            builder.Services.AddSingleton<ProducttypeRepository>();
            builder.Services.AddSingleton<ProductRepository>();
            builder.Services.AddScoped<GebruikerRepository>();
            builder.Services.AddTransient<Aspnetroles>();

            // AppShell
            builder.Services.AddSingleton<AppShell>();

            // ViewModels
            builder.Services.AddScoped<LoginViewModel>();
            builder.Services.AddTransient<BestellingViewModel>();
            builder.Services.AddTransient<AfrekenViewModel>();
            builder.Services.AddTransient<ProductViewModel>();
            builder.Services.AddTransient<GebruikerViewModel>();
            builder.Services.AddTransient<BestellingDetailViewModel>();

            // Views
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<BestellingPage>();
            builder.Services.AddTransient<AfrekenPage>();
            builder.Services.AddTransient<ProductPage>();
            builder.Services.AddTransient<GebruikerPage>();
            builder.Services.AddTransient<BestellingDetailPage>();
            builder.Services.AddTransient<LogoutPage>();

            return builder.Build();
        }
    }
}
