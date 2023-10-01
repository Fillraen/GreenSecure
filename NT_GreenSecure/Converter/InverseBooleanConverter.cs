// Page de code 3 - InverseBooleanConverter.cs
// Ce convertisseur inverse simplement une valeur booléenne.
// Si la valeur d'entrée est true, il renvoie false, et vice versa.
// Si la conversion échoue, il renvoie false par défaut.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace NT_GreenSecure.Converter
{
    public class InverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }
}