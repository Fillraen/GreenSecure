using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using NT_GreenSecure.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using System.ComponentModel;
using Xamarin.Forms;
using Rg.Plugins.Popup;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NT_GreenSecure.Views.Popup;
using NT_GreenSecure.Views;

namespace NT_GreenSecure.Droid.Renderers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class TodoShellItemRenderer : ShellItemRenderer
    {
        FrameLayout _shellOverlay;
        FrameLayout _shellOverlayAll;
        BottomNavigationView _bottomView;

        public TodoShellItemRenderer(IShellContext shellContext) : base(shellContext)
        {
            // S'abonner aux changements de propriété
            if (ShellItem != null)
            {
                ShellItem.PropertyChanged += ShellItem_PropertyChanged;
            }
        }

        private void ShellItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Shell.TabBarIsVisibleProperty.PropertyName)
            {
                UpdateBottomViewVisibility();
            }
        }

        private void UpdateBottomViewVisibility()
        {

            var isVisible = Shell.GetTabBarIsVisible(ShellItem);
            var visibility = isVisible ? ViewStates.Visible : ViewStates.Gone;

            _bottomView.Visibility = visibility;
            _shellOverlay.Visibility = visibility;
            _shellOverlayAll.Visibility = visibility;


        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var outerlayout = base.OnCreateView(inflater, container, savedInstanceState);
            _bottomView = outerlayout.FindViewById<BottomNavigationView>(Resource.Id.bottomtab_tabbar);
            _shellOverlay = outerlayout.FindViewById<FrameLayout>(Resource.Id.bottomtab_tabbar_container);
            _shellOverlayAll = outerlayout.FindViewById<FrameLayout>(Resource.Id.bottomtab_container_all);

            if (ShellItem is TodoTabBar todoTabBar && todoTabBar.LargeTab != null)
                SetupLargeTab();

            UpdateBottomViewVisibility();

            return outerlayout;
        }

        private async void SetupLargeTab()
        {
            var todoTabBar = (TodoTabBar)ShellItem;
            var layout = new FrameLayout(Context);

            var imageSourceHandler = Xamarin.Forms.Internals.Registrar.Registered.GetHandlerForObject<IImageSourceHandler>(todoTabBar.LargeTab.Icon);
            Bitmap bitmap = await imageSourceHandler.LoadImageAsync(todoTabBar.LargeTab.Icon, this.Context);

            var image = new ImageView(Context);
            image.SetImageBitmap(bitmap);

            image.Click += (sender, args) =>
            {
                //open the popup
                HandleImageClick();
            };

            layout.AddView(image);

            var lp = new FrameLayout.LayoutParams(175, 175);
            _bottomView.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);
            lp.BottomMargin = _bottomView.MeasuredHeight / 2;

            layout.LayoutParameters = lp;

            _shellOverlay.RemoveAllViews();
            _shellOverlay.AddView(layout);
        }

        private async void HandleImageClick()
        {
            // Ouvrir la popup
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushPopupAsync(new NT_GreenSecure.Views.Popup.AddPasswordPopup());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Désabonner des événements pour éviter les fuites de mémoire
                if (ShellItem != null)
                {
                    ShellItem.PropertyChanged -= ShellItem_PropertyChanged;
                }
            }
            base.Dispose(disposing);
        }
    }
}