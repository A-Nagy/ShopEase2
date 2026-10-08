using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopEase2.Helpers.Converters
{
    public sealed class RatingToStarsConverter :
      IValueConverter
    {
        public object Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            int filled =
                value is double rating

                    ? Math.Clamp(
                        (int)Math.Round(rating),
                        0,
                        5)

                    : 0;

            return
                new string(
                    '★',
                    filled) +
                new string(
                    '☆',
                    5 - filled);
        }

        public object ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
