using ShopEase2.Models;
using ShopEase2.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Services.Mock
{
    public sealed class MockAuthService : IAuthService
    {
        private readonly ISessionService _sessionService;

        public MockAuthService(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

         public async Task<Result<AuthSession>> LoginAsync(
            string username,
            string password,
            CancellationToken ct = default)
        {
            await Task.Delay(800, ct);

            if (!string.Equals(
                    username.Trim(),
                    "emilys",
                    StringComparison.OrdinalIgnoreCase)
                || password != "emilyspass")
            {
                return Result<AuthSession>.Fail( "Invalid username or password.");
            }

            return Result<AuthSession>.Ok(new AuthSession {
                    UserId = 1,
                    Username = "emilys",
                    Email = "emily.johnson@x.dummyjson.com",
                    FirstName = "Emily",
                    LastName = "Johnson",
                    AccessToken = "mock-access-token" });
        }

        public async Task<Result<UserProfile>> GetCurrentUserAsync(
            CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            AuthSession? current =  await _sessionService.GetAsync();

            if (current is null)
            {
                return Result<UserProfile>.Fail(
                    "There is no active session.");
            }

            return Result<UserProfile>.Ok(
                new UserProfile
                {
                    Id = current.UserId,
                    Username = current.Username,
                    Email = current.Email,
                    FirstName = current.FirstName,
                    LastName = current.LastName,
                    Image = current.Image
                });
        }
    }
}
