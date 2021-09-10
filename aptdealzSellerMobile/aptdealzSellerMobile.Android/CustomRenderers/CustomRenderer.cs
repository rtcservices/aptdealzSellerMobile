using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Widget;
using aptdealzSellerMobile.Droid.CustomRenderers;
using aptdealzSellerMobile.Extention;
using aptdealzSellerMobile.Utility;
using dotMorten.Xamarin.Forms;
using dotMorten.Xamarin.Forms.Platform.Android;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(LabelCustomRenderer))]
[assembly: ExportRenderer(typeof(ExtEntry), typeof(EntryCustomRenderer))]
[assembly: ExportRenderer(typeof(Picker), typeof(PickerCustomRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(EditorCustomRenderer))]
[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(CustomButtonRender))]
[assembly: ExportRenderer(typeof(ExtAutoSuggestBox), typeof(AutoSuggestBoxCustomRenderer))]
[assembly: ExportRenderer(typeof(ExtDatePicker), typeof(CustomDatePickerRender))]

namespace aptdealzSellerMobile.Droid.CustomRenderers
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class LabelCustomRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            try
            {
                base.OnElementChanged(e);
                string fontFamily = e.NewElement?.FontFamily;
                if (!Common.EmptyFiels(fontFamily))
                {
                    var label = (TextView)Control; // for example
                    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                    label.Typeface = font;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Droid/LabelRenderer: " + ex.Message);
            }
        }
    }

    public class EntryCustomRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            try
            {
                base.OnElementChanged(e);
                string fontFamily = e.NewElement?.FontFamily;
                if (!Common.EmptyFiels(fontFamily))
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

                var maxLenght = e.NewElement?.MaxLength;
                if (maxLenght == 1)
                {
                    Control.Gravity = Android.Views.GravityFlags.Center;
                }
                else
                {
                    Control.Gravity = Android.Views.GravityFlags.CenterVertical;
                }

                if (Control != null)
                {
                    Control.ImeOptions = Android.Views.InputMethods.ImeAction.Done;
                    //Control.InputType = Android.Text.InputTypes.ClassText | Android.Text.InputTypes.TextVariationVisiblePassword | Android.Text.InputTypes.TextFlagMultiLine;
                    //Control.SetTypeface(Typeface.Default, TypefaceStyle.Normal);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Droid/EntryRenderer: " + ex.Message);
            }
        }
    }

    public class PickerCustomRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            try
            {
                base.OnElementChanged(e);
                string fontFamily = e.NewElement?.FontFamily;
                if (!Common.EmptyFiels(fontFamily))
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
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Droid/PickerRenderer: " + ex.Message);
            }
        }
    }

    public class EditorCustomRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            try
            {
                base.OnElementChanged(e);
                var fontFamily = e.NewElement?.FontFamily;
                if (!string.IsNullOrEmpty(fontFamily))
                {
                    var label = (TextView)Control; // for example
#pragma warning disable CS0618 // Type or member is obsolete
                    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
#pragma warning restore CS0618 // Type or member is obsolete
                    label.Typeface = font;
                }
                var nativeedittextfield = (Android.Widget.EditText)this.Control;
                GradientDrawable gd = new GradientDrawable();
                nativeedittextfield.Background = gd;

                Control.SetPadding(0, 0, 0, 0);
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Droid/EditorRenderer: " + ex.Message);
            }
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

                if (!Common.EmptyFiels(e.NewElement?.FontFamily))
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    var font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, e.NewElement.FontFamily + ".otf");
#pragma warning restore CS0618 // Type or member is obsolete
                    Control.Typeface = font;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Droid/ButtonRenderer: " + ex.Message);
            }
        }
    }

    public class AutoSuggestBoxCustomRenderer : AutoSuggestBoxRenderer
    {
        public AutoSuggestBoxCustomRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestBox> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (Control != null)
                {
                    GradientDrawable gd = new GradientDrawable();
                    gd.SetColor(global::Android.Graphics.Color.Transparent);
#pragma warning disable CS0618 // Type or member is obsolete
                    Control.SetBackgroundDrawable(gd);
#pragma warning restore CS0618 // Type or member is obsolete
                    Control.SetRawInputType(InputTypes.TextFlagNoSuggestions);
                    gd.SetSize(15, 15);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Droid/AutoSuggestBoxRenderer: " + ex.Message);
            }
        }
    }

    public class CustomDatePickerRender : DatePickerRenderer
    {
        public CustomDatePickerRender(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                return;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                base.OnElementPropertyChanged(sender, e);

                if (Control == null)
                {
                    return;
                }

                var nativeEditTextField = Control;
                GradientDrawable gd = new GradientDrawable();
                gd.SetCornerRadius(0);

                nativeEditTextField.Background = gd;
                Control.SetPadding(0, 0, 0, 0);

                ExtDatePicker element = Element as ExtDatePicker;
                if (element.NullableDate.HasValue)
                    Control.Text = element.NullableDate.Value.ToString(element.Format);
                else
                    Control.Text = string.Empty;
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("Droid/DatePickerRenderer: " + ex.Message);
            }
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete

}