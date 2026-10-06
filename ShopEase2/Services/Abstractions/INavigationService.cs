using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Services.Abstractions
{
    public interface INavigationService
    {
   Task GoToAsync( 
        string route,
        IDictionary<string, object>? parameters = null);

        Task GoBackAsync();
    }
}
