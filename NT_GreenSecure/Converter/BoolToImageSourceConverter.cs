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
