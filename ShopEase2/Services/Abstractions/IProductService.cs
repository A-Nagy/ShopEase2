using ShopEase2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Services.Abstractions
{
    public interface IProductService
    {
        Task<Result<ProductPage>> GetProductsAsync( ProductQuery query, CancellationToken ct = default);

        Task<Result<Product>> GetProductAsync(int id, CancellationToken ct = default);

        Task<Result<IReadOnlyList<Category>>> GetCategoriesAsync( CancellationToken ct = default);
    }
}
