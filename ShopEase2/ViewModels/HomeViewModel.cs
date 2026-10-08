using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using ShopEase2.Constants;
using ShopEase2.Models;
using ShopEase2.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShopEase2.ViewModels
{
    public sealed partial class HomeViewModel :BaseViewModels
    {
        private readonly IProductService _products;

        private bool _initialized;
        private string? _categorySlug;

        private int _skip;
        private int _total;
        private int _version;

        private bool _suppressSearch;

        private CancellationTokenSource?
            _searchDelay;

        private CancellationTokenSource?
            _request;

        public ObservableCollection<Product>
            Products
        { get; } = [];

        public ObservableCollection<CategoryItem>
            Categories
        { get; } = [];

        public IReadOnlyList<BannerItem>
            Banners
        { get; } =
        [
            new(
            "Daily discoveries",
            "Browse products from the API",
            "Primary"),

        new(
            "Everyday essentials",
            "Find your next product",
            "Accent"),

        new(
            "Live catalog",
            "Products arrive from DummyJSON",
            "Success")
        ];

        public IReadOnlyList<SortOption>
            SortOptions
        { get; } =
        [
            new(
            "Default",
            null,
            null),

        new(
            "Price: low to high",
            "price",
            "asc"),

        new(
            "Price: high to low",
            "price",
            "desc"),

        new(
            "Top rated",
            "rating",
            "desc"),

        new(
            "Title A-Z",
            "title",
            "asc")
        ];

        public IReadOnlyList<PriceLimitOption>
            PriceLimits
        { get; } =
        [
            new(
            "Any price",
            null),

        new(
            "Up to $50",
            50m),

        new(
            "Up to $100",
            100m),

        new(
            "Up to $500",
            500m)
        ];

        [ObservableProperty]
        private string _searchText = "";

        [ObservableProperty]
        private SortOption _selectedSort;

        [ObservableProperty]
        private PriceLimitOption _selectedPriceLimit;

        [ObservableProperty]
        private double _minRating;

        [ObservableProperty]
        private bool _isRefreshing;

        [ObservableProperty]
        private bool _isLoadingMore;

        [ObservableProperty]
        private bool _isEmpty;

        [ObservableProperty]
        private bool _canLoadMore;

        public HomeViewModel(
            IProductService products)
        {
            _products = products;

            _selectedSort =
                SortOptions[0];

            _selectedPriceLimit =
                PriceLimits[0];

            Title = "Discover";
        }

        private ProductQuery Query(
            int skip) =>
            new()
            {
                Search =
                    _searchText,

                CategorySlug =
                    _categorySlug,

                SortBy =
                    _selectedSort.SortBy,

                Order =
                    _selectedSort.Order,

                MaxPrice =
                    _selectedPriceLimit.Maximum,

                MinRating =
                    Math.Round(
                        _minRating),

                Skip =
                    skip,

                Limit =
                    AppConstants.PageSize
            };

        [RelayCommand]
        private async Task LoadAsync()
        {
            if (_initialized)
            {
                return;
            }

            IsBusy = true;
            ErrorMessage = null;

            try
            {
                Result<
                    IReadOnlyList<Category>>
                    result =
                        await _products
                            .GetCategoriesAsync();

                if (!result.IsSuccess ||
                    result.Data is null)
                {
                    ErrorMessage =
                        result.Error ??
                        "Cannot load categories.";

                    return;
                }

                Categories.Clear();

                Categories.Add(
                    new CategoryItem
                    {
                        Name = "All",
                        Slug = "",
                         
                    });

                foreach (Category category
                         in result.Data)
                {
                    Categories.Add(
                        new CategoryItem
                        {
                            Name =
                                category.Name,

                            Slug =
                                category.Slug
                        });
                }

                _initialized = true;

                await ReloadAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ReloadAsync(
            CancellationToken cancellationToken =
                default)
        {
            _request?.Cancel();

            using var request =
                CancellationTokenSource
                    .CreateLinkedTokenSource(
                        cancellationToken);

            _request = request;

            int version =
                ++_version;

            IsBusy = true;
            ErrorMessage = null;

            try
            {
                Result<ProductPage> result =
                    await _products
                        .GetProductsAsync(
                            Query(0),
                            request.Token);

                if (version != _version)
                {
                    return;
                }

                if (!result.IsSuccess ||
                    result.Data is null)
                {
                    Products.Clear();

                    IsEmpty = false;
                    CanLoadMore = false;

                    ErrorMessage =
                        result.Error ??
                        "Cannot load products.";

                    return;
                }

                Products.Clear();

                foreach (Product product
                         in result.Data.Products)
                {
                    Products.Add(product);
                }

                _skip =
                    result.Data.Products.Count;

                _total =
                    result.Data.Total;

                IsEmpty =
                    Products.Count == 0;

                CanLoadMore =
                    _skip < _total;
            }
            catch (OperationCanceledException)
                when (request.IsCancellationRequested)
            {
            }
            finally
            {
                if (version == _version)
                {
                    IsBusy = false;

                    if (ReferenceEquals(
                            _request,
                            request))
                    {
                        _request = null;
                    }
                }
            }
        }

        // NEW (Day 4.B4): Debounced search.
        partial void OnSearchTextChanged(
            string value)
        {
            if (!_initialized ||
                _suppressSearch)
            {
                return;
            }

            CancelSearchDelay();

            var delay =
                new CancellationTokenSource();

            _searchDelay = delay;

            _ =
                DebouncedReloadAsync(
                    delay);
        }

        private async Task DebouncedReloadAsync(
            CancellationTokenSource delay)
        {
            try
            {
                await Task.Delay(
                    AppConstants.SearchDebounceMs,
                    delay.Token);

                await ReloadAsync(
                    delay.Token);
            }
            catch (OperationCanceledException)
                when (delay.IsCancellationRequested)
            {
            }
            finally
            {
                if (ReferenceEquals(
                        _searchDelay,
                        delay))
                {
                    _searchDelay = null;
                }

                delay.Dispose();
            }
        }

        private void CancelSearchDelay()
        {
            _searchDelay?.Cancel();
            _searchDelay = null;
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            CancelSearchDelay();

            await ReloadAsync();
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            IsRefreshing = true;

            try
            {
                await ReloadAsync();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        private Task ApplyFiltersAsync() =>
            ReloadAsync();

        [RelayCommand]
        private async Task SelectCategoryAsync(
            CategoryItem? item)
        {
            if (item is null)
            {
                return;
            }

            //foreach (CategoryItem category
            //         in Categories)
            //{
            //    category.IsSelected =
            //        category == item;
            //}

            _categorySlug =
                item.Slug;

            CancelSearchDelay();

            await ReloadAsync();
        }

        [RelayCommand]
        private async Task ClearFiltersAsync()
        {
            CancelSearchDelay();

            _suppressSearch = true;

            SearchText = "";

            _suppressSearch = false;

            _categorySlug = null;

            //foreach (CategoryItem category
            //         in Categories)
            //{
            //    category.IsSelected =
            //        category.Slug.Length == 0;
            //}

            SelectedSort =
                SortOptions[0];

            SelectedPriceLimit =
                PriceLimits[0];

            MinRating = 0;

            await ReloadAsync();
        }

        [RelayCommand]
        private async Task LoadMoreAsync()
        {
            if (!CanLoadMore ||
                IsBusy ||
                IsLoadingMore)
            {
                return;
            }

            IsLoadingMore = true;

            int version =
                _version;

            try
            {
                Result<ProductPage> result =
                    await _products
                        .GetProductsAsync(
                            Query(_skip),
                            _request?.Token ??
                            CancellationToken.None);

                if (version != _version)
                {
                    return;
                }

                if (!result.IsSuccess ||
                    result.Data is null)
                {
                    ErrorMessage =
                        result.Error ??
                        "Cannot load more products.";

                    return;
                }

                foreach (Product product
                         in result.Data.Products)
                {
                    Products.Add(product);
                }

                _skip +=
                    result.Data.Products.Count;

                _total =
                    result.Data.Total;

                CanLoadMore =
                    result.Data.Products.Count > 0 &&
                    _skip < _total;
            }
            catch (OperationCanceledException)
                when (version != _version)
            {
            }
            finally
            {
                IsLoadingMore = false;
            }
        }

        [RelayCommand]
        private Task RetryAsync() =>
            _initialized
                ? ReloadAsync()
                : LoadAsync();
    }
}
