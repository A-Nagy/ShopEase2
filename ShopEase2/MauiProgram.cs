using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using ShopEase2.Constants;
using ShopEase2.Services.Abstractions;
using ShopEase2.Services.Api;
using ShopEase2.Services.Local;
using ShopEase2.Services.Mock;
using ShopEase2.View;
using ShopEase2.ViewModels;
using ShopEase2.Views;

namespace ShopEase2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
              .UseMauiCommunityToolkit() 
              .ConfigureFonts(fonts =>
              {
                  fonts.AddFont(
                      "OpenSansRegular.ttf",
                      "OpenSansRegular");

                  fonts.AddFont(
                      "OpenSansSemibold.ttf",
                      "OpenSansSemibold");
              });
            //builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            //    {
            //        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            //        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            //    });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

            builder.Services.AddSingleton< IAuthService,MockAuthService>();

            builder.Services.AddSingleton<ISessionService,InMemorySessionService>();

            builder.Services.AddSingleton<INavigationService,ShellNavigationService>();

            builder.Services.AddSingleton<IDialogService,MauiDialogService>();
            builder.Services.AddSingleton<IConnectivityService,MauiConnectivityService>();

            builder.Services.AddHttpClient<IProductService,ApiProductService>(client =>
            {
               client.BaseAddress = new Uri(AppConstants.ApiBaseUrl);

               client.Timeout = TimeSpan.FromSeconds(15);
           });


            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<SplashViewModel>();
            builder.Services.AddTransient<SplashPage>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<HomePage>();

            return builder.Build();
        }
    }
}
