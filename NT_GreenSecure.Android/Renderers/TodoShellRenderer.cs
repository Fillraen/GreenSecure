using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NT_GreenSecure.Droid.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(Shell), typeof(TodoShellRenderer))]
namespace NT_GreenSecure.Droid.Renderers
{
    public class TodoShellRenderer : ShellRenderer
    {
        public TodoShellRenderer(Context context) : base(context)
        {

        }
        protected override IShellItemRenderer CreateShellItemRenderer(ShellItem shellItem)
        {
            return new TodoShellItemRenderer(this);
        }
    }
}