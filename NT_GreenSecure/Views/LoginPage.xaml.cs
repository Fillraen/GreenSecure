using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NT_GreenSecure.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private async void Signup_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new Popup.RegisterPopup());
        }
        private async void Login_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new Popup.AuthPopup());
        }
    }
}