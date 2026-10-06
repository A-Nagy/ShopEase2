using ShopEase2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Services.Abstractions
{
    public interface ISessionService
    {
        Task<AuthSession?> GetAsync();

        Task SaveAsync(AuthSession session);

        Task ClearAsync();
    }
}
