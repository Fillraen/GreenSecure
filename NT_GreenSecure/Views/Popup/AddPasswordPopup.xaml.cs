﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NT_GreenSecure.ViewModels.Popup;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NT_GreenSecure.Views.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPasswordPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public AddPasswordPopup()
        {
            InitializeComponent();
            this.BindingContext = new AddPasswordViewModel();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<CredentialDetailViewModel>(this, "RefreshList");
        }
    }
}