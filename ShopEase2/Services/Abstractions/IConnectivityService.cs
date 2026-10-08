using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Services.Abstractions
{
    public interface IConnectivityService
    {
        bool IsConnected { get; }
    }
}
