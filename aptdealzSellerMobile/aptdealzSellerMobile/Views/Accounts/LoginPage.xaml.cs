using Acr.UserDialogs;
using aptdealzSellerMobile.API;
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
        #region Objects
        private bool isChecked = false;
        #endregion

        #region Constructor
        public LoginPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        public bool Validations()
        {
            bool isValid = false;
            if (Common.EmptyFiels(txtUsername.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Email_Phone);
            }
            if (Common.EmptyFiels(txtPassword.Text))
            {
                Common.DisplayErrorMessage(Constraints.Required_Password);
            }
            //else if (!Common.IsValidPassword(txtPassword.Text))
            //{
            //    DisplayAlert(Constraints.Alert, String.Format("The {0} must be at least {1} characters long and should have atleast one capital leter, special character ({2}) and digit.", "Password", 8, "#$^+=!*()@%&"), Constraints.Ok);
            //}
            else
            {
                isValid = true;
            }
            return isValid;
        }

        async void LoginUser()
        {
            try
            {
                if (Validations())
                {
                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    Model.Request.Authenticate mAuthenticate = new Model.Request.Authenticate();
                    mAuthenticate.Email = txtUsername.Text;
                    mAuthenticate.Password = txtPassword.Text;

                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    var mResponse = await authenticationAPI.SellerAuth(mAuthenticate);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                        if (jObject != null)
                        {
                            var mSeller = jObject.ToObject<Model.Reponse.Seller>();
                            if (mSeller != null)
                            {
                                Settings.UserId = mSeller.Id;
                                Common.Token = mSeller.JwToken;
                                Settings.UserToken = mSeller.JwToken;
                                Settings.RefreshToken = mSeller.RefreshToken;
                                Settings.LoginTrackingKey = mSeller.LoginTrackingKey == "00000000-0000-0000-0000-000000000000" ? Settings.LoginTrackingKey : mSeller.LoginTrackingKey;

                                if (isChecked)
                                {
                                    Settings.EmailAddress = txtUsername.Text;
                                    Settings.Password = txtPassword.Text;
                                    Settings.UserToken = mSeller.JwToken;
                                }
                                else
                                {
                                    Settings.EmailAddress = string.Empty;
                                    Settings.Password = string.Empty;
                                    Settings.UserToken = string.Empty;
                                }
                                App.Current.MainPage = new MainTabbedPage("Home");

                                txtUsername.Text = string.Empty;
                                txtPassword.Text = string.Empty;
                                isChecked = false;
                                imgCheck.Source = Constraints.CheckBox_UnChecked;
                            }
                        }
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
                Common.DisplayErrorMessage("LoginPage/LoginUser: " + ex.Message);
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

        private void StkRemember_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (imgCheck.Source.ToString().Replace("File: ", "") == Constraints.CheckBox_Checked)
                {
                    isChecked = false;
                    imgCheck.Source = Constraints.CheckBox_UnChecked;
                }
                else
                {
                    isChecked = true;
                    imgCheck.Source = Constraints.CheckBox_Checked;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/StkRemember_Tapped: " + ex.Message);

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

        private void BtnLogin_Tapped(object sender, EventArgs e)
        {
            Common.BindAnimation(frame: frmBtnLogin);
            LoginUser();
        }

        private void ImgPassword_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectedImage = Convert.ToString(imgEye.Source).Replace("File: ", string.Empty);
                if (selectedImage == Constraints.Img_Hide)
                {
                    txtPassword.IsPassword = false;
                    imgEye.Source = Constraints.Img_Show;
                }
                else
                {
                    txtPassword.IsPassword = true;
                    imgEye.Source = Constraints.Img_Hide;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/ImgPassword_Tapped: " + ex.Message);
            }
        }
        #endregion
    }
}