using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Constants
{
  public static class Routes
    {
        public const string Splash = "/splash";
        public const string Register = "register";
        public const string Login = "/login";
        public const string Home = "/home";
        public const string Setting = "/setting";
        public const string Products = "/products";
        public const string ProductDetails = "/products/{id}";
        public const string Cart = "/cart";
        public const string Checkout = "/checkout";
        public const string OrderConfirmation = "/order-confirmation";
    }
}
