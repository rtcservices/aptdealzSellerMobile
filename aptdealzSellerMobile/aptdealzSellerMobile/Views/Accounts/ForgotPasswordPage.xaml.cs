using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Utility;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        #region Constructor
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        private bool Validation()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtEmail.Text))
                {
                    BoxEmail.BackgroundColor = (Color)App.Current.Resources["LightRed"];
                    Common.DisplayErrorMessage(Constraints.Required_Email);
                }
                else if (!txtEmail.Text.IsValidEmail())
                {
                    Common.DisplayErrorMessage(Constraints.InValid_Email);
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ForgotPasswordPage/Validation: " + ex.Message);
            }
            return isValid;
        }

        private async void SendOtpByEmail()
        {
            try
            {
                if (Validation())
                {
                    txtEmail.Text = txtEmail.Text.Trim();
                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);

                    var mResponse = await authenticationAPI.SendOtpByEmail(txtEmail.Text);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        Common.DisplaySuccessMessage(mResponse.Message);
                        await Navigation.PushAsync(new EnterOtpPage(txtEmail.Text));
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ForgotPasswordPage/SendOtpByEmail: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        #endregion

        #region Events
        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            Navigation.PopAsync();
        }

        private void ResetPassword_Tapped(object sender, EventArgs e)
        {
            try
            {
                Common.BindAnimation(button: btnResetPassword);
                SendOtpByEmail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ForgotPasswordPage/ResetPassword_Tapped: " + ex.Message);
            }
        }

        private void txtEmail_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (Extention.ExtEntry)sender;
            if (!Common.EmptyFiels(entry.Text))
            {
                BoxEmail.BackgroundColor = (Color)App.Current.Resources["LightGray"];
            }
        }
        #endregion
    }
}