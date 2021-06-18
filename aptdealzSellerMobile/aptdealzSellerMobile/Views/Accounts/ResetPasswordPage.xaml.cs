using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.SplashScreen;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasswordPage : ContentPage
    {
        protected string EmailAddress;

        #region Ctor
        public ResetPasswordPage(string Email)
        {
            InitializeComponent();
            EmailAddress = Email;
        }
        #endregion

        #region Methods
        public bool Validation()
        {
            bool isValid = false;
            if (Common.EmptyFiels(txtNewPassword.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_NewPassword);
            }
            else if (!txtNewPassword.Text.IsValidPassword())
            {
                Common.DisplayErrorMessage(Constraints.InValid_NewPassword);
            }
            else if (Common.EmptyFiels(txtConfirmPassword.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_ConfirmPassword);
            }
            else if (!txtConfirmPassword.Text.IsValidPassword())
            {
                Common.DisplayErrorMessage(Constraints.InValid_ConfirmPassword);
            }
            else if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                Common.DisplayErrorMessage(Constraints.Same_New_Confirm_Password);
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }

        async void ResetPassword()
        {
            try
            {
                if (Validation())
                {
                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    ResetPassword mResetPassword = new ResetPassword();

                    mResetPassword.Email = EmailAddress;
                    mResetPassword.Token = Common.Token;
                    mResetPassword.Password = txtNewPassword.Text;

                    var mResponse = await authenticationAPI.ResetPassword(mResetPassword);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        Common.DisplaySuccessMessage(mResponse.Message);
                        App.Current.MainPage = new NavigationPage(new WelcomePage(true));
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
                Common.DisplayErrorMessage("ResetPasswordPage/ResetPassword: " + ex.Message);
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
            Common.BindAnimation(image: ImgBack);
            Navigation.PopAsync();
        }

        private void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnSubmit);
            ResetPassword();
        }
        #endregion

        private void ImgNewPassword_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectedImage = Convert.ToString(imgNewPass.Source).Replace("File: ", string.Empty);
                if (selectedImage == Constraints.Img_Hide)
                {
                    txtNewPassword.IsPassword = false;
                    imgNewPass.Source = Constraints.Img_Show;
                }
                else
                {
                    txtNewPassword.IsPassword = true;
                    imgNewPass.Source = Constraints.Img_Hide;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/ImgPassword_Tapped: " + ex.Message);
            }
        }

        private void ImgConfirmPassword_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectedImage = Convert.ToString(imgConfirmPass.Source).Replace("File: ", string.Empty);
                if (selectedImage == Constraints.Img_Hide)
                {
                    txtConfirmPassword.IsPassword = false;
                    imgConfirmPass.Source = Constraints.Img_Show;
                }
                else
                {
                    txtConfirmPassword.IsPassword = true;
                    imgConfirmPass.Source = Constraints.Img_Hide;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/ImgPassword_Tapped: " + ex.Message);
            }
        }
    }
}