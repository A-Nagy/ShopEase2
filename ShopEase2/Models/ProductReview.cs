using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Models
{
    public sealed class ProductReview
    {
        public int Rating { get; set; }

        public string Comment { get; set; } = "";

        public DateTime Date { get; set; }

        public string ReviewerName { get; set; } = "";
    }
}
