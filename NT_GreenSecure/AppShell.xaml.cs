using NT_GreenSecure.ViewModels;
using NT_GreenSecure.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace NT_GreenSecure
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
