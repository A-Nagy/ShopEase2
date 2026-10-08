using ShopEase2.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Models
{
    public sealed record ProductQuery
    {
        public string? Search { get; init; }

        public string? CategorySlug { get; init; }

        public string? SortBy { get; init; }

        public string? Order { get; init; }

        // NEW (Day 4.B4): Local combined filters.
        public decimal? MaxPrice { get; init; }

        public double MinRating { get; init; }

        public int Skip { get; init; }

        public int Limit { get; init; } = AppConstants.PageSize;
    }
}
