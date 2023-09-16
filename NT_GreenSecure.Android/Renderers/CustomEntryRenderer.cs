using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics.Drawables;
using Android.Text;
using NT_GreenSecure.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using NT_GreenSecure.Droid.Renderers;
using Java.Util;
using Android.Graphics;
using AndroidX.Core.Content;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]

namespace NT_GreenSecure.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        CustomEntry element;

        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetColor(Android.Graphics.Color.Transparent);

                // Suppression de la bordure
                gd.SetStroke(0, Android.Graphics.Color.Transparent);

                this.Control.SetBackground(gd);
                this.Control.SetPadding(20, 0, 0, 0);

                CustomEntry customEntry = (CustomEntry)e.NewElement;
                if (customEntry.IsPasswordFlag)
                {
                    this.Control.InputType = InputTypes.TextVariationVisiblePassword;
                }
            }

            if (e.OldElement != null || e.NewElement == null)
                return;

            element = (CustomEntry)this.Element;

            var editText = this.Control;
            if (!string.IsNullOrEmpty(element.Image))
            {
                switch (element.ImageAlignment)
                {
                    case ImageAlignment.Left:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(GetDrawable(element.Image), null, null, null);
                        break;
                    case ImageAlignment.Right:
                        editText.SetCompoundDrawablesWithIntrinsicBounds(null, null, GetDrawable(element.Image), null);
                        break;
                }
            }
            editText.CompoundDrawablePadding = 15;

        }

        private BitmapDrawable GetDrawable(string imageEntryImage)
        {
            int resID = Resources.GetIdentifier(imageEntryImage, "drawable", this.Context.PackageName);
            if (resID == 0)
            {
                resID = Resources.GetIdentifier(imageEntryImage, "drawable", "android");
            }
            if (resID == 0)
            {
                throw new InvalidOperationException("Drawable resource not found.");
            }

            var drawable = ContextCompat.GetDrawable(this.Context, resID);
            if (drawable == null)
            {
                throw new InvalidOperationException("Drawable cannot be null");
            }

            var bitmap = ((BitmapDrawable)drawable).Bitmap;
            return new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, element.ImageWidth * 2, element.ImageHeight * 2, true));
        }
    }
}
