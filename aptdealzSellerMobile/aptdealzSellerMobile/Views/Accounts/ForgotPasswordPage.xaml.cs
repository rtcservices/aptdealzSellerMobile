using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Utility;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        #region [ Constructor ]
        public ForgotPasswordPage()
        {
            InitializeComponent();
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

        private bool Validation()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtEmail.Text))
                {
                    BoxEmail.BackgroundColor = (Color)App.Current.Resources["appColor3"];
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

        private async Task SendOtpByEmail()
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

        #region [ Events ]
        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            await Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PopAsync();
        }

        private async void ResetPassword_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(button: btnResetPassword);
                await SendOtpByEmail();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ForgotPasswordPage/ResetPassword_Tapped: " + ex.Message);
            }
        }

        private void txtEmail_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                var entry = (Extention.ExtEntry)sender;
                if (!Common.EmptyFiels(entry.Text))
                {
                    BoxEmail.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ForgotPasswordPage/txtEmail_Unfocused: " + ex.Message);
            }
        }
        #endregion
    }
}