using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Services.Abstractions
{
    public interface IDialogService
    {
        Task ShowAlertAsync(
        string title,
        string message,
        string cancel = "OK");

        Task<bool> ConfirmAsync(
            string title,
            string message,
            string accept,
            string cancel);

        Task ShowToastAsync(string message);
    }
}
