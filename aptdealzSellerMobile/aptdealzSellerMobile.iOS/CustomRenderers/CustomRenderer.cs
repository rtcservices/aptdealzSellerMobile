using aptdealzSellerMobile.Extention;
using aptdealzSellerMobile.iOS.CustomRenderers;
using aptdealzSellerMobile.Utility;
using CoreGraphics;
using dotMorten.Xamarin.Forms;
using dotMorten.Xamarin.Forms.Platform.iOS;
using Foundation;
using System;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Label), typeof(LabelCustomRenderer))]
[assembly: ExportRenderer(typeof(ExtEntry), typeof(EntryCustomRenderer))]
[assembly: ExportRenderer(typeof(Picker), typeof(PickerCustomRenderer))]
[assembly: ExportRenderer(typeof(Editor), typeof(EditorCustomRenderer))]
[assembly: ExportRenderer(typeof(ExtKeyboard), typeof(KeyboardViewRenderer))]
[assembly: ExportRenderer(typeof(ExtAutoSuggestBox), typeof(AutoSuggestBoxCustomRenderer))]
[assembly: ExportRenderer(typeof(ExtDatePicker), typeof(CustomeDatePickerRenderer))]

namespace aptdealzSellerMobile.iOS.CustomRenderers
{
    public class LabelCustomRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (Control != null)
                {
                    if (e.NewElement != null && e.NewElement.Text != null)
                    {
                        if (!string.IsNullOrEmpty(Element.FontFamily))
                            Control.Font = UIFont.FromName(this.Element.FontFamily, (nfloat)e.NewElement.FontSize);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("iOS/LabelRenderer: " + ex.Message);
            }
        }
    }

    public class EntryCustomRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (Control != null)
                {
                    Control.BorderStyle = UITextBorderStyle.None;

                    Control.Layer.BackgroundColor = Color.Transparent.ToCGColor();
                    Control.Layer.BorderColor = Color.Transparent.ToCGColor();
                    Control.Layer.CornerRadius = (nfloat)0.0;
                    Control.LeftView = new UIView(new CGRect(0, 0, 0, 0));
                    Control.LeftViewMode = UITextFieldViewMode.Always;
                    Control.RightView = new UIView(new CGRect(0, 0, 0, 0));
                    Control.RightViewMode = UITextFieldViewMode.Always;

                    var keyboard = e.NewElement?.Keyboard;
                    if (keyboard != Keyboard.Numeric)
                    {
                        Control.AutocapitalizationType = UITextAutocapitalizationType.Sentences;
                        Control.KeyboardType = UIKeyboardType.ASCIICapable;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("iOS/EntryRenderer: " + ex.Message);
            }
        }
    }

    public class PickerCustomRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (Control != null)
                {
                    Control.Layer.BackgroundColor = Color.White.ToCGColor();
                    Control.Layer.CornerRadius = (nfloat)0.0;
                    Control.LeftView = new UIView(new CGRect(0, 0, 5, 0));
                    Control.LeftViewMode = UITextFieldViewMode.Always;
                    Control.RightView = new UIView(new CGRect(0, 0, 5, 0));
                    Control.RightViewMode = UITextFieldViewMode.Always;
                    this.Control.BorderStyle = UITextBorderStyle.None;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("iOS/PickerRenderer: " + ex.Message);
            }
        }
    }

    public class EditorCustomRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (Control != null)
                {
                    Control.Layer.BorderColor = Color.Transparent.ToCGColor();
                    Control.Layer.BorderWidth = 0;
                    Control.AutocapitalizationType = UITextAutocapitalizationType.Sentences;
                    Control.KeyboardType = UIKeyboardType.ASCIICapable;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("iOS/EditorRenderer: " + ex.Message);
            }
        }
    }

    public class KeyboardViewRenderer : ViewRenderer
    {
        NSObject _keyboardShowObserver;
        NSObject _keyboardHideObserver;
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                RegisterForKeyboardNotifications();
            }

            if (e.OldElement != null)
            {
                UnregisterForKeyboardNotifications();
            }
        }

        void RegisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver == null)
                _keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
            if (_keyboardHideObserver == null)
                _keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
        }

        void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {

            NSValue result = (NSValue)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
            CGSize keyboardSize = result.RectangleFValue.Size;
            if (Element != null)
            {
                Element.Margin = new Thickness(0, 0, 0, keyboardSize.Height); //push the entry up to keyboard height when keyboard is activated

            }
        }

        void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                Element.Margin = new Thickness(0); //set the margins to zero when keyboard is dismissed
            }

        }


        void UnregisterForKeyboardNotifications()
        {
            if (_keyboardShowObserver != null)
            {
                _keyboardShowObserver.Dispose();
                _keyboardShowObserver = null;
            }

            if (_keyboardHideObserver != null)
            {
                _keyboardHideObserver.Dispose();
                _keyboardHideObserver = null;
            }
        }
    }

    public class AutoSuggestBoxCustomRenderer : AutoSuggestBoxRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<AutoSuggestBox> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (Control != null)
                {
                    Control.Layer.BackgroundColor = Color.Transparent.ToCGColor();
                    Control.Layer.BorderColor = Color.Transparent.ToCGColor();
                    Control.Layer.CornerRadius = (nfloat)0.0;
                    Control.IsSuggestionListOpen = false;
                    Control.ShowBottomBorder = false;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("iOS/AutoSuggestBoxRenderer: " + ex.Message);
            }
        }
    }

    public class CustomeDatePickerRenderer : DatePickerRenderer
    {
        public static void Init() { }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                base.OnElementPropertyChanged(sender, e);
                if (Control != null)
                {
                    Control.Layer.BorderWidth = 0;
                    Control.BorderStyle = UITextBorderStyle.None;

                    ExtDatePicker element = Element as ExtDatePicker;
                    if (element.NullableDate.HasValue)
                        Control.Text = element.NullableDate.Value.ToString(element.Format);
                    else
                        Control.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("iOS/DatePickerRenderer: " + ex.Message);
            }
        }
    }
}