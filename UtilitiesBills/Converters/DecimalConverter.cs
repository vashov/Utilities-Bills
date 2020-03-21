using System;
using System.Globalization;
using Xamarin.Forms;

namespace UtilitiesBills.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "0";
            }
            decimal num = (decimal)value;
            return num.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            if (string.IsNullOrEmpty(strValue))
            {
                strValue = "0";
            }
            if (decimal.TryParse(strValue, out decimal resultdecimal))
            {
                return resultdecimal;
            }
            return 0;
        }
    }
}
