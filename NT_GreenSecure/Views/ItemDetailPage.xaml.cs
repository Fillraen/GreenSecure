using NT_GreenSecure.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace NT_GreenSecure.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}