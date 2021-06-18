using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.MasterData;
using aptdealzSellerMobile.Views.SplashScreen;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterOtpPage : ContentPage
    {
        #region Objects
        private string EmailAddress;
        private string OTPString;
        AuthenticationAPI authenticationAPI;
        #endregion

        #region Constructor
        public EnterOtpPage(string Email)
        {
            InitializeComponent();
            EmailAddress = Email;
            authenticationAPI = new AuthenticationAPI();
            ResendButtonEnable();
        }

        void ResendButtonEnable()
        {
            BtnResentOtp.IsEnabled = false;
            int i = 120;

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                BtnResentOtp.Text = i + " sec";
                if (i == 0)
                {
                    BtnResentOtp.IsEnabled = true;
                    BtnResentOtp.Text = "Resend OTP";
                    return false;
                }
                i--;
                return true;
            });
        }

        #endregion

        #region Methods        
        bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(TxtOtpOne.Text)
                  || Common.EmptyFiels(TxtOtpTwo.Text)
                  || Common.EmptyFiels(TxtOtpThree.Text)
                  || Common.EmptyFiels(TxtOtpFour.Text)
                  || Common.EmptyFiels(TxtOtpFive.Text)
                  || Common.EmptyFiels(TxtOtpSix.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_VerificationCode);
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/Validations: " + ex.Message);
            }
            return isValid;
        }

        async void SubmitOTP()
        {
            try
            {
                if (Validations())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);

                    AuthenticateEmail mAuthenticateEmail = new AuthenticateEmail();
                    mAuthenticateEmail.Email = EmailAddress;
                    OTPString = TxtOtpOne.Text + TxtOtpTwo.Text + TxtOtpThree.Text + TxtOtpFour.Text + TxtOtpFive.Text + TxtOtpSix.Text;
                    mAuthenticateEmail.Otp = OTPString;

                    var mResponse = await authenticationAPI.ValidateOtpForResetPassword(mAuthenticateEmail);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        Common.DisplaySuccessMessage(mResponse.Message);
                        var Token = (string)mResponse.Data;
                        Common.Token = Token;

                        await Navigation.PushAsync(new ResetPasswordPage(EmailAddress));
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);
                    }
                }
                else
                {
                    Common.DisplayErrorMessage(Constraints.Required_VerificationCode);
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/FrmSubmit_Tapped: " + ex.Message);
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        async void ResentOTP()
        {
            try
            {
                UserDialogs.Instance.ShowLoading(Constraints.Loading);
                var mResponse = await authenticationAPI.SendOtpByEmail(EmailAddress);
                if (mResponse != null && mResponse.Succeeded)
                {
                    Common.DisplaySuccessMessage(mResponse.Message);
                }
                else
                {
                    if (mResponse != null)
                        Common.DisplayErrorMessage(mResponse.Message);
                    else
                        Common.DisplayErrorMessage(Constraints.Something_Wrong);
                }

                ResendButtonEnable();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/BtnResentOtp_Tapped: " + ex.Message);
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
            Navigation.PopAsync();
        }

        private void FrmSubmit_Tapped(object sender, EventArgs e)
        {
            SubmitOTP();
        }

        private void BtnResentOtp_Tapped(object sender, EventArgs e)
        {
            ResentOTP();
        }

        private void TxtOtpOne_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpOne.Text))
                TxtOtpTwo.Focus();
        }

        private void TxtOtpTwo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpTwo.Text))
                TxtOtpThree.Focus();
            else
                TxtOtpOne.Focus();
        }

        private void TxtOtpThree_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpThree.Text))
                TxtOtpFour.Focus();
            else
                TxtOtpTwo.Focus();
        }

        private void TxtOtpFour_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpFour.Text))
                TxtOtpFive.Focus();
            else
                TxtOtpThree.Focus();
        }

        private void TxtOtpFive_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpFive.Text))
                TxtOtpSix.Focus();
            else
                TxtOtpFour.Focus();
        }

        private void TxtOtpSix_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Common.EmptyFiels(TxtOtpSix.Text))
            {
                TxtOtpSix.Unfocus();
                frmSubmit.BackgroundColor = (Color)App.Current.Resources["Green"];
            }
            else
                TxtOtpFive.Focus();
        }
        #endregion
    }
}