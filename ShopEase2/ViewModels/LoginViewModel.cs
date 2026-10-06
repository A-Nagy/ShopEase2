using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ShopEase2.Models;
using ShopEase2.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.ViewModels
{
    public sealed partial class LoginViewModel : BaseViewModels
    {
        private readonly IAuthService    _authService;
        private readonly ISessionService _sessionService;
        private readonly IDialogService  _dialogService;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _username = "";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        private string _password = "";

        [ObservableProperty]
        private bool _isPasswordHidden = true;

         public LoginViewModel(
            IAuthService authService,
            ISessionService sessionService,
            INavigationService navigationService,
            IDialogService dialogService,
            IMessenger messenger)
        {
            _authService = authService;
            _sessionService = sessionService;
            _dialogService = dialogService;

            ArgumentNullException.ThrowIfNull(navigationService);

            ArgumentNullException.ThrowIfNull(messenger);

            Title = "Login";

            PropertyChanged += (_, args) =>
            {
                if (args.PropertyName ==
                    nameof(IsBusy))
                {
                    LoginCommand.NotifyCanExecuteChanged();
                }
            };
        }

        private bool CanLogin() =>IsNotBusy &&
            !string.IsNullOrWhiteSpace(Username) &&
            !string.IsNullOrWhiteSpace(Password);

        [RelayCommand(CanExecute = nameof(CanLogin))]
        private async Task LoginAsync()
        {
            if (!CanLogin())
            {
                return;
            }

            IsBusy = true;
            ErrorMessage = null;

            try
            {
                Result<AuthSession> result =
                    await _authService.LoginAsync(
                        Username.Trim(),
                        Password);

                if (!result.IsSuccess ||
                    result.Data is null)
                {
                    ErrorMessage =
                        result.Error ??
                        "Sign in failed.";

                    return;
                }

                await _sessionService.SaveAsync(
                    result.Data);

                 await _dialogService.ShowAlertAsync(
                    "ShopEase",
                    "Demo sign-in succeeded.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void UseDemoAccount()
        {
            Username = "emilys";
            Password = "emilyspass";
        }

        [RelayCommand]
        private void TogglePasswordVisibility() =>
            IsPasswordHidden = !IsPasswordHidden;
    }
}
