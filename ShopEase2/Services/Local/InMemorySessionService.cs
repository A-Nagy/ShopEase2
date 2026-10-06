using ShopEase2.Models;
using ShopEase2.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Services.Local
{
    public sealed class InMemorySessionService :ISessionService
    {
        private AuthSession? _session;
        public Task<AuthSession?> GetAsync() =>
    Task.FromResult(_session);
        public Task SaveAsync(AuthSession session)
        {
            _session = session;
            return Task.CompletedTask;
        }

        public Task ClearAsync()
        {
            _session = null;
            return Task.CompletedTask;
        }
    }
}
