using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShopEase2.Constants;
using ShopEase2.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.ViewModels
{
    public sealed partial class SplashViewModel : BaseViewModels
    {
        private readonly INavigationService _navigationService;
        private bool _started;

        public SplashViewModel(INavigationService navigationService,
                               ISessionService sessionService,
                               IAuthService authService,
                               IMessenger messenger)
        {
            _navigationService = navigationService;

            ArgumentNullException.ThrowIfNull(sessionService);

            ArgumentNullException.ThrowIfNull(authService);

            ArgumentNullException.ThrowIfNull(messenger);

            Title = "ShopEase";
        }

        [RelayCommand]
        private async Task InitializeAsync()
        {
            if (_started)
            {
                return;
            }

            _started = true;
            IsBusy = true;

            try
            {
                await Task.Delay(AppConstants.MinSplashMs);

                await _navigationService.GoToAsync(Routes.Home);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}