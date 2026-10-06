using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using ShopEase2.Services.Abstractions;
using ShopEase2.Services.Local;
using ShopEase2.Services.Mock;
using ShopEase2.View;
using ShopEase2.ViewModels;

namespace ShopEase2
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
            builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

            builder.Services.AddSingleton< IAuthService,MockAuthService>();

            builder.Services.AddSingleton<ISessionService,InMemorySessionService>();

            builder.Services.AddSingleton<INavigationService,ShellNavigationService>();

            builder.Services.AddSingleton<IDialogService,MauiDialogService>();

            builder.Services.AddTransient<LoginViewModel>();

            builder.Services.AddTransient<LoginPage>();

            return builder.Build();
        }
    }
}
