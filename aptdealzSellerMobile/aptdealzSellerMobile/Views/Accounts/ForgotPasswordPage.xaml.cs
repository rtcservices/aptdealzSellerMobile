using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private async void ResetPassword_Tapped(object sender, EventArgs e)
        {
            try
            {
                await btnResetPassword.ScaleTo(0.9, 100, Easing.Linear);
                await btnResetPassword.ScaleTo(1.0, 100, Easing.Linear);
            }
            catch (Exception ex)
            {


            }
        }
    }
}