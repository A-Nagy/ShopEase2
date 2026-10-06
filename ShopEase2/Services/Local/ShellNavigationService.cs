using ShopEase2.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Services.Local
{
    public sealed class ShellNavigationService : INavigationService
    {
  public Task GoToAsync( string route,
                                      IDictionary<string, object>? parameters = null)
        {
            return parameters is null
                ? Shell.Current.GoToAsync(route)
                : Shell.Current.GoToAsync(route, parameters);
        }

        public Task GoBackAsync() =>
            Shell.Current.GoToAsync("..");
    }
}
