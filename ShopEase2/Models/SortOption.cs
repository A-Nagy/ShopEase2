using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Models
{
    public sealed record SortOption (string Label,
                                     string? SortBy,
                                     string? Order);
    
}
