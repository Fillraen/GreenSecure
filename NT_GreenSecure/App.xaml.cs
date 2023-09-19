﻿using NT_GreenSecure.Services;
using NT_GreenSecure.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NT_GreenSecure
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            
            MainPage = new AppShell();
            string token = Preferences.Get("access_token", string.Empty);
            DateTime tokenExpiry;
           
            bool isDateParsed = DateTime.TryParse(Preferences.Get("token_expiry", string.Empty), out tokenExpiry);
            
            if (!string.IsNullOrEmpty(token) && isDateParsed)
            {
                if (DateTime.UtcNow < tokenExpiry)
                {
                    MainPage = new HomePage();
                }
                else
                {
                   Preferences.Remove("access_token");
                   Preferences.Remove("token_expiry");
                   App.Current.MainPage.DisplayAlert("Session expirée", "Veuillez vous reconnecter", "OK");
                   MainPage = new LoginPage();
                }
            }
            else
            {
                MainPage = new LoginPage();
            }
               
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}