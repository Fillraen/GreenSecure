// Page de code 1 - BoolToImageSourceConverter.cs
// Ce convertisseur transforme un booléen en une source d'image Xamarin.Forms.
// Si la valeur d'entrée est true, il renvoie l'icône pour afficher le mot de passe, sinon, l'icône pour masquer le mot de passe.
// Par défaut, il renvoie l'icône pour masquer le mot de passe.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace NT_GreenSecure.Converter
{
    public class BoolToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isPasswordVisible)
            {
                return isPasswordVisible ? "./icon_showPWD.png" : "./icon_hidePWD.png";
            }
            return "./icon_hidePWD.png"; // Par défaut
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}