using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NT_GreenSecure.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public RegisterPopup()
        {
            InitializeComponent();

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
    }
}