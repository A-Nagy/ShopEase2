using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Models
{
    public sealed class ProductPage
    {
        public List<Product> Products { get; set; } = [];

        public int Total { get; set; }

        public int Skip { get; set; }

        public int Limit { get; set; }
    }
}
