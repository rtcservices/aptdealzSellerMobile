using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Utility;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterOtpPage : ContentPage
    {
        #region [ Objects ]
        private string EmailAddress;
        private string OTPString;
        private AuthenticationAPI authenticationAPI;
        #endregion

        #region [ Constructor ]
        public EnterOtpPage(string Email)
        {
            InitializeComponent();
            EmailAddress = Email;
            authenticationAPI = new AuthenticationAPI();
            ResendButtonEnable();
        }
        #endregion

        #region [ Methods ]    
        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Dispose();
        }

        private void ResendButtonEnable()
        {
            try
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
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("EnterOtpPage/ResendButtonEnable: " + ex.Message);
            }
        }

        private bool Validations()
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

        private async Task SubmitOTP()
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

        private async Task ResentOTP()
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

        #region [ Events ]
        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PopAsync();
        }

        private async void BtnSubmit_Tapped(object sender, EventArgs e)
        {
            var Tab = (Button)sender;
            if (Tab.IsEnabled)
            {
                try
                {
                    Tab.IsEnabled = false; Common.BindAnimation(button: BtnSubmit);
                    await SubmitOTP();
                }
                catch (Exception ex)
                {
                    Common.DisplayErrorMessage("EnterOtpPage/BtnSubmit_Tapped: " + ex.Message);
                }
                finally
                {
                    Tab.IsEnabled = true;
                }
            }
        }

        private async void BtnResentOtp_Tapped(object sender, EventArgs e)
        {
            await ResentOTP();
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
                BtnSubmit.BackgroundColor = (Color)App.Current.Resources["appColor1"];
            }
            else
                TxtOtpFive.Focus();
        }
        #endregion
    }
}