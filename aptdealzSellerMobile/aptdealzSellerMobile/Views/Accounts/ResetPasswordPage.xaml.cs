using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Extention;
using aptdealzSellerMobile.Model.Request;
using aptdealzSellerMobile.Utility;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasswordPage : ContentPage
    {
        #region [ Objects ]
        private string EmailAddress;
        #endregion

        #region [ Ctor ]
        public ResetPasswordPage(string Email)
        {
            InitializeComponent();
            EmailAddress = Email;
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
                if (Common.EmptyFiels(txtNewPassword.Text) || Common.EmptyFiels(txtConfirmPassword.Text))
                {
                    RequiredFields();
                    isValid = false;
                }

                if (Common.EmptyFiels(txtNewPassword.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_NewPassword);
                }
                else if (!txtNewPassword.Text.IsValidPassword())
                {
                    DisplayAlert(Constraints.Alert, String.Format("The {0} must be at least {1} characters long and should have atleast one capital leter, special character ({2}) and digit.", "Password", 8, "#$^+=!*()@%&"), Constraints.Ok);
                    //Common.DisplayErrorMessage(Constraints.InValid_NewPassword);
                }
                else if (Common.EmptyFiels(txtConfirmPassword.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_ConfirmPassword);
                }
                else if (!txtConfirmPassword.Text.IsValidPassword())
                {
                    DisplayAlert(Constraints.Alert, String.Format("The {0} must be at least {1} characters long and should have atleast one capital leter, special character ({2}) and digit.", "Password", 8, "#$^+=!*()@%&"), Constraints.Ok);
                    //Common.DisplayErrorMessage(Constraints.InValid_ConfirmPassword);
                }
                else if (txtNewPassword.Text != txtConfirmPassword.Text)
                {
                    Common.DisplayErrorMessage(Constraints.Same_New_Confirm_Password);
                }
                else
                {
                    isValid = true;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ResetPasswordPage/Validation: " + ex.Message);
            }
            return isValid;
        }

        private void RequiredFields()
        {
            try
            {
                if (Common.EmptyFiels(txtNewPassword.Text))
                {
                    BoxNewPassword.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }

                if (Common.EmptyFiels(txtConfirmPassword.Text))
                {
                    BoxConfirmPassword.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ResetPasswordPage/EnableRequiredFields: " + ex.Message);
            }
        }

        private void FieldsTrim()
        {
            txtNewPassword.Text = txtNewPassword.Text.Trim();
            txtConfirmPassword.Text = txtConfirmPassword.Text.Trim();
        }

        private async Task ResetPassword()
        {
            try
            {
                if (Validation())
                {
                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    FieldsTrim();
                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    ResetPassword mResetPassword = new ResetPassword();

                    mResetPassword.Email = EmailAddress;
                    mResetPassword.Token = Common.Token;
                    mResetPassword.Password = txtNewPassword.Text;

                    var mResponse = await authenticationAPI.ResetPassword(mResetPassword);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        Common.DisplaySuccessMessage(mResponse.Message);
                        App.Current.MainPage = new NavigationPage(new Views.Accounts.LoginPage());
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

        #region [ Events ]
        private async void ImgBack_Tapped(object sender, EventArgs e)
        {
            await Common.BindAnimation(imageButton: ImgBack);
            await Navigation.PopAsync();
        }

        private async void BtnSubmit_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(button: BtnSubmit);
                await ResetPassword();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ResetPasswordPage/BtnSubmit_Clicked: " + ex.Message);
            }
        }

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
                Common.DisplayErrorMessage("ResetPasswordPage/ImgPassword_Tapped: " + ex.Message);
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
                Common.DisplayErrorMessage("ResetPasswordPage/ImgPassword_Tapped: " + ex.Message);
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                var entry = (ExtEntry)sender;
                if (!Common.EmptyFiels(entry.Text))
                {
                    if (entry.ClassId == "NewPassword")
                    {
                        BoxNewPassword.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "ConfirmPassword")
                    {
                        BoxConfirmPassword.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("ResetPasswordPage/Entry_Unfocused: " + ex.Message);
            }
        }
        #endregion
    }
}