using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using aptdealzSellerMobile.Droid.CustomRenderers;
using aptdealzSellerMobile.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(LabelCustomRenderer))]
[assembly: ExportRenderer(typeof(ExtEntry), typeof(EntryCustomRenderer))]
[assembly: ExportRenderer(typeof(Picker), typeof(PickerCustomRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(EditorCustomRenderer))]
[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(CustomButtonRender))]

namespace aptdealzSellerMobile.Droid.CustomRenderers
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class LabelCustomRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var label = (TextView)Control; // for example
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                label.Typeface = font;
            }
        }
    }

    public class EntryCustomRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var textbox = (TextView)Control; // for example
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                textbox.Typeface = font;
            }

            var editText = (Android.Widget.EditText)this.Control;
            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(0);
            gd.SetColor(Android.Graphics.Color.Transparent);
            editText.Background = gd;

            Control.SetPadding(0, 0, 0, 0);
            Control.Gravity = Android.Views.GravityFlags.CenterVertical;
        }
    }

    public class PickerCustomRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var label = (TextView)Control; // for example
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                label.Typeface = font;
            }

            var nativeedittextfield = (Android.Widget.EditText)this.Control;
            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(0);
            gd.SetColor(Android.Graphics.Color.Transparent);
            nativeedittextfield.Background = gd;

            Control.SetPadding(0, 0, 0, 0);
            Control.Gravity = Android.Views.GravityFlags.CenterVertical;
        }
    }

    public class EditorCustomRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var label = (TextView)Control; // for example
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                label.Typeface = font;
            }

            var nativeedittextfield = (Android.Widget.EditText)Control;
            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(0);
            gd.SetColor(Android.Graphics.Color.Transparent);
            nativeedittextfield.Background = gd;

            Control.SetPadding(0, 0, 0, 0);
            Control.Gravity = Android.Views.GravityFlags.Start;
        }
    }

    public class CustomButtonRender : ButtonRenderer
    {
        public CustomButtonRender(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            try
            {
                base.OnElementChanged(e);
                var button = Control;
                button.SetAllCaps(false);

                if (!string.IsNullOrEmpty(e.NewElement?.FontFamily))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, e.NewElement.FontFamily + ".otf");
#pragma warning restore CS0618 // Type or member is obsolete
                    Control.Typeface = font;
                }
            }
            catch (Exception ex)
            {
                //Common.DisplayErrorMessage("Droid/CustomButtonRender: " + ex.Message);
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete

}