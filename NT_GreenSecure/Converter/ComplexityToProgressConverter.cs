// Page de code 2 - ComplexityToProgressConverter.cs
// Ce convertisseur transforme un entier (représentant la complexité) en une valeur entre 0 et 1.
// Il divise simplement la complexité par 100 pour obtenir la valeur entre 0 et 1.
// Si la conversion échoue, il renvoie 0 par défaut.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace NT_GreenSecure.Converter
{
    public class ComplexityToProgressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int complexity)
            {
                return complexity / 100.0; // Convertir à une valeur entre 0 et 1.
            }
            return 0; // Valeur par défaut si la conversion échoue.
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Nous n'implémentons pas la conversion inverse dans ce cas.
        }
    }
}