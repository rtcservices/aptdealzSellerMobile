using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.MainTabbedPages;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        #region Constructor
        public LoginPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        // create events here
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void StkRemember_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (imgCheck.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Check)
                {
                    imgCheck.Source = Constraints.CheckBox_UnCheck;
                }
                else
                {
                    imgCheck.Source = Constraints.CheckBox_Check;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void StkSignup_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignupPage());
        }

        private void ForgotPassword_Click(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage());
        }

        private async void btnLogin_Tapped(object sender, EventArgs e)
        {
            await frmBtnLogin.ScaleTo(0.9, 100, Easing.Linear);
            await frmBtnLogin.ScaleTo(1.0, 100, Easing.Linear);

            await Navigation.PushAsync(new MainTabbedPage(false));
        }
        #endregion
    }
}