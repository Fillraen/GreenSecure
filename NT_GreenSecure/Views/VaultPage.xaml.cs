using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NT_GreenSecure.ViewModels;
using NT_GreenSecure.ViewModels.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NT_GreenSecure.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VaultPage : ContentPage
    {
        public VaultPage()
        {
            InitializeComponent();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CredentialDetailViewModel>(this, "RefreshList");
            MessagingCenter.Unsubscribe<AddPasswordViewModel>(this, "RefreshList");
        }
    }
}