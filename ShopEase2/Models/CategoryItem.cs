using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Models
{
    public sealed class CategoryItem
    {
        public string Slug { get; init; } = "";

        public string Name { get; init; } = "";

 
        private bool _isSelected { get; set; } = false;
    }
}
