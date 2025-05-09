using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using PsicoAgenda.Services;

namespace PsicoAgenda
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddSingleton<AuthService>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();           
        }
    }
}