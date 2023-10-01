using Android.OS;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using NT_GreenSecure.Controls;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Extensions;
using NT_GreenSecure.Views.Popup;

namespace NT_GreenSecure.Droid.Renderers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class TodoShellItemRenderer : ShellItemRenderer
    {
        FrameLayout _shellOverlay;
        BottomNavigationView _bottomView;
        public TodoShellItemRenderer(IShellContext shellContext) : base(shellContext)
        {

        }

        public override Android.Views.View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var outerlayout = base.OnCreateView(inflater, container, savedInstanceState);
            _bottomView = outerlayout.FindViewById<BottomNavigationView>(Resource.Id.bottomtab_tabbar);
            _shellOverlay = outerlayout.FindViewById<FrameLayout>(Resource.Id.bottomtab_tabbar_container);

            if (ShellItem is TodoTabBar todoTabBar && todoTabBar.LargeTab != null)
            {
                SetupLargeTab();
            }
            
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

            var lp = new FrameLayout.LayoutParams(125, 125);
            _bottomView.Measure((int)MeasureSpecMode.Unspecified, (int)MeasureSpecMode.Unspecified);
            lp.BottomMargin = _bottomView.MeasuredHeight / 2;

            layout.LayoutParameters = lp;

            _shellOverlay.RemoveAllViews();
            _shellOverlay.AddView(layout);
        }

        private async void HandleImageClick()
        {
            // Ouvrir la popup
            await Xamarin.Forms.Application.Current.MainPage.Navigation.PushPopupAsync(new AddPasswordPopup());
        }

    }
}