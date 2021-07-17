using System;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Extention
{
    public class ExtTimePicker : TimePicker
    {
        public const string NullableDatePropertyName = "NullableTime";

        public static readonly BindableProperty NullableDateProperty = BindableProperty.Create<ExtTimePicker, TimeSpan?>
            (i => i.NullableTime, null, BindingMode.TwoWay, null, propertyChanged: OnTimeChanged, propertyChanging: null, coerceValue: null, defaultValueCreator: null);


        public TimeSpan? NullableTime
        {
            get
            {
                return (TimeSpan?)this.GetValue(NullableDateProperty);
            }
            set
            {
                this.SetValue(NullableDateProperty, value);
            }
        }

        public const string NullTextPropertyName = "NullText";

        public static readonly BindableProperty NullTextProperty = BindableProperty.Create<ExtTimePicker, string>(i => i.NullText, default(string), BindingMode.TwoWay);

        public string NullText
        {
            get
            {
                return (string)this.GetValue(NullTextProperty);
            }
            set
            {
                this.SetValue(NullTextProperty, value);
            }
        }

        public const string DisplayBorderPropertyName = "DisplayBorder";

        public static readonly BindableProperty DisplayBorderProperty = BindableProperty.Create<ExtTimePicker, bool>(i => i.DisplayBorder, default(bool), BindingMode.TwoWay);

        public bool DisplayBorder
        {
            get
            {
                return (bool)this.GetValue(DisplayBorderProperty);
            }
            set
            {
                this.SetValue(DisplayBorderProperty, value);
            }
        }

        public ExtTimePicker()
        {
            this.PropertyChanged += ExtTimePicker_PropertyChanged;
            this.Time = DateTime.Now.TimeOfDay;
            this.Format = "hh:mm tt";
        }

        private void ExtTimePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private static void OnTimeChanged(BindableObject bindable, TimeSpan? oldvalue, TimeSpan? newvalue)
        {
            var nullableTimePicker = bindable as ExtTimePicker;
            if (nullableTimePicker != null && oldvalue != newvalue)
            {
                nullableTimePicker.Time = newvalue.Value;
            }
        }

        public static readonly BindableProperty EnterTextProperty = BindableProperty.Create(propertyName: "Placeholder", returnType: typeof(string), declaringType: typeof(TimePicker), defaultValue: default(string));
        public string Placeholder
        {
            get;
            set;
        }

    }
}
