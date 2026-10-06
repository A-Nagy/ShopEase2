using ShopEase2.Services.Abstractions;
using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Services.Local
{
    public sealed class MauiDialogService :IDialogService
    {
        public Task ShowAlertAsync(
        string title,
        string message,
        string cancel = "OK") =>
        Shell.Current.DisplayAlert(
            title,
            message,
            cancel);

        public Task<bool> ConfirmAsync(
            string title,
            string message,
            string accept,
            string cancel) =>
            Shell.Current.DisplayAlert(
                title,
                message,
                accept,
                cancel);

        public Task ShowToastAsync(string message) =>
            Shell.Current.DisplayAlert(
                "ShopEase",
                message,
                "OK");
    }
}
