using ShopEase2.Models;
using ShopEase2.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopEase2.Services.Api
{
    public sealed class ApiProductService : IProductService
    {
        private readonly HttpClient _http;
        private readonly IConnectivityService _connectivity;

        public ApiProductService( HttpClient http,  IConnectivityService connectivity)
        {
            _http = http;
            _connectivity = connectivity;
        }

        // NEW (Day 4.B2): Shared GET pipeline.
        private async Task<Result<T>> GetAsync<T>(string path,CancellationToken ct)
        {
            if (!_connectivity.IsConnected)
            {
                return Result<T>.Fail("No internet connection. Check the device network and retry.");
            }

            try
            {
                using HttpResponseMessage response =
                    await _http.GetAsync( path, HttpCompletionOption.ResponseHeadersRead,
                        ct);

                if (!response.IsSuccessStatusCode)
                {
                    return Result<T>.Fail( await ApiJson.ErrorAsync( response,ct));
                }

                T? value = await response.Content.ReadFromJsonAsync<T>(ApiJson.Options, ct);

                return value is null
                    ? Result<T>.Fail("The server returned an empty response.")
                    : Result<T>.Ok(value);
            }
            catch (OperationCanceledException)
                when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (OperationCanceledException)
            {
                return Result<T>.Fail("The request timed out. Try again.");
            }
            catch (HttpRequestException)
            {
                return Result<T>.Fail( "The server could not be reached. Try again.");
            }
            catch (JsonException)
            {
                return Result<T>.Fail( "The server returned data we could not read.");
            }
        }

        public async Task<Result<ProductPage>>
            GetProductsAsync(ProductQuery query,  CancellationToken ct = default)
        {
            bool hasSearch =!string.IsNullOrWhiteSpace( query.Search);

            bool hasCategory =!string.IsNullOrWhiteSpace(query.CategorySlug);

            /*
             * NEW (Day 4.B4):
             * Combined filtering uses a complete matched set locally.
             */
            bool localFilter =
                (hasSearch && hasCategory) ||
                query.MaxPrice is not null ||
                query.MinRating > 0 ||
                ((hasSearch || hasCategory) &&
                 query.SortBy is not null);

            string path = hasSearch? "products/search?q=" + Uri.EscapeDataString( query.Search!.Trim())
                        : hasCategory? "products/category/" +Uri.EscapeDataString( query.CategorySlug!.Trim()): "products";

            int limit =localFilter? 0: Math.Max(1, query.Limit);

            int skip =
                localFilter
                    ? 0
                    : Math.Max(
                        0,
                        query.Skip);

            path +=
                $"{(path.Contains('?') ? '&' : '?')}limit={limit}&skip={skip}";

            if (!localFilter &&
                !string.IsNullOrWhiteSpace(
                    query.SortBy))
            {
                path +=
                    "&sortBy=" +
                    Uri.EscapeDataString(
                        query.SortBy);
            }

            if (!localFilter &&
                !string.IsNullOrWhiteSpace(
                    query.Order))
            {
                path +=
                    "&order=" +
                    Uri.EscapeDataString(
                        query.Order);
            }

            Result<ProductPage> result =
                await GetAsync<ProductPage>(
                    path,
                    ct);

            if (!result.IsSuccess ||
                result.Data is null ||
                !localFilter)
            {
                return result;
            }

            IEnumerable<Product> selected =
                result.Data.Products;

            if (hasSearch &&
                hasCategory)
            {
                selected =
                    selected.Where(
                        p => p.Category.Equals(
                            query.CategorySlug,
                            StringComparison.OrdinalIgnoreCase));
            }

            if (query.MaxPrice is decimal maxPrice)
            {
                selected =
                    selected.Where(
                        p => p.DiscountedPrice <=
                             maxPrice);
            }

            if (query.MinRating > 0)
            {
                selected =
                    selected.Where(
                        p => p.Rating >=
                             query.MinRating);
            }

            selected =
                (query.SortBy, query.Order) switch
                {
                    ("price", "asc") =>
                        selected.OrderBy(
                            p => p.Price),

                    ("price", "desc") =>
                        selected.OrderByDescending(
                            p => p.Price),

                    ("rating", "desc") =>
                        selected.OrderByDescending(
                            p => p.Rating),

                    ("title", "asc") =>
                        selected.OrderBy(
                            p => p.Title),

                    _ =>
                        selected.OrderBy(
                            p => p.Id)
                };

            List<Product> filtered =
                selected.ToList();

            int pageSkip =
                Math.Max(
                    0,
                    query.Skip);

            int pageLimit =
                Math.Max(
                    1,
                    query.Limit);

            return Result<ProductPage>.Ok(
                new ProductPage
                {
                    Products =
                        filtered
                            .Skip(pageSkip)
                            .Take(pageLimit)
                            .ToList(),

                    Total =
                        filtered.Count,

                    Skip =
                        pageSkip,

                    Limit =
                        pageLimit
                });
        }

        public Task<Result<Product>>
            GetProductAsync(
                int id,
                CancellationToken ct = default) =>
            GetAsync<Product>(
                $"products/{id}",
                ct);

        public async Task<
            Result<IReadOnlyList<Category>>>
            GetCategoriesAsync(
                CancellationToken ct = default)
        {
            Result<List<Category>> result =
                await GetAsync<List<Category>>(
                    "products/categories",
                    ct);

            return result.IsSuccess &&
                   result.Data is not null

                ? Result<
                    IReadOnlyList<Category>>
                    .Ok(result.Data)

                : Result<
                    IReadOnlyList<Category>>
                    .Fail(
                        result.Error ??
                        "Cannot load categories.");
        }
    }
}
