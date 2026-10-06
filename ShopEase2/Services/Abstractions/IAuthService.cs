using ShopEase2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Services.Abstractions
{
    public interface IAuthService
    {
        Task<Result<AuthSession>> LoginAsync(
       string username,
       string password,
       CancellationToken ct = default);

        Task<Result<UserProfile>> GetCurrentUserAsync(
            CancellationToken ct = default);
    }
}
