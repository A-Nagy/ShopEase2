using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Models
{
    public sealed class UserProfile
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string Email { get; set; } = "";

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string? Image { get; set; }

        public string FullName =>
            $"{FirstName} {LastName}".Trim();
    }
}
