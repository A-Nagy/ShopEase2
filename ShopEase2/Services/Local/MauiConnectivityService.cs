using ShopEase2.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Services.Local
{
    public sealed class MauiConnectivityService :IConnectivityService
    {
        // NEW (Day 4.B2): Network access does not prove the server is reachable.
        public bool IsConnected =>  Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
    }
}
