using Microsoft.Extensions.Logging;

namespace Companion
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

            // HttpClient
            builder.Services.AddSingleton(new HttpClient());

            // ViewModels
            builder.Services.AddTransient<BaseViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<MenukaartViewModel>();
            builder.Services.AddTransient<WinkelwagenViewModel>();
            builder.Services.AddTransient<EvenementViewModel>();

            // Views
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegistreerPage>();
            builder.Services.AddTransient<MenukaartPage>();
            builder.Services.AddTransient<EvenementPage>();
            builder.Services.AddTransient<WinkelwagenPage>();

            return builder.Build();
        }
    }
}
