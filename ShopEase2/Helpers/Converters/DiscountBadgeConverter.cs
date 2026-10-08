using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Helpers.Converters
{
    public sealed class DiscountBadgeConverter :IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture) =>
            value is double percent &&
            percent > 0

                ? $"-{percent:0}%"

                : "";

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
