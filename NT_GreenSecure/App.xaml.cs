using NT_GreenSecure.Services;
using NT_GreenSecure.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NT_GreenSecure
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
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
