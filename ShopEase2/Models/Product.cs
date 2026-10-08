using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ShopEase2.Models
{
    public sealed class Product
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public string Category { get; set; } = "";

        public decimal Price { get; set; }

        public double DiscountPercentage { get; set; }

        public double Rating { get; set; }

        public int Stock { get; set; }

        public string? Brand { get; set; }

        public List<string> Tags { get; set; } = [];

        public string Thumbnail { get; set; } = "";

        public List<string> Images { get; set; } = [];

        public List<ProductReview> Reviews { get; set; } = [];

        public string? AvailabilityStatus { get; set; }

        public string? ShippingInformation { get; set; }

        public string? WarrantyInformation { get; set; }

        public string? ReturnPolicy { get; set; }

        [JsonIgnore]
        public decimal DiscountedPrice => Price * (1 - (decimal)DiscountPercentage / 100);

        [JsonIgnore]
        public bool HasDiscount => DiscountPercentage > 0;
    }
}
