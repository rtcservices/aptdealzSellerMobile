using Acr.UserDialogs;
using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Extention;
using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.Model.Reponse;
using aptdealzSellerMobile.Utility;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        #region [ Objects ]
        private bool isEmail = false;
        #endregion

        #region [ Constructor ]
        public LoginPage()
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
            UserDialogs.Instance.HideLoading();
            base.OnDisappearing();
            Dispose();
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await DisplayAlert(Constraints.Alert, Constraints.DoYouWantToExit, Constraints.Yes, Constraints.No);
                        if (result)
                        {
                            Xamarin.Forms.DependencyService.Get<ICloseAppOnBackButton>().CloseApp("LoginPage");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        public bool Validations()
        {
            bool isValid = false;
            try
            {
                if (Common.EmptyFiels(txtUsername.Text) || Common.EmptyFiels(txtPassword.Text))
                {
                    RequiredFields();
                    isValid = false;
                }

                if (Common.EmptyFiels(txtUsername.Text))
                {
                    Common.DisplayErrorMessage(Constraints.Required_Email_Phone);
                }
                else if (txtUsername.Text.Contains("@") || txtUsername.Text.Contains("."))
                {
                    if (!txtUsername.Text.IsValidEmail())
                    {
                        Common.DisplayErrorMessage(Constraints.InValid_Email);
                    }
                    else
                    {
                        isEmail = true;
                        isValid = true;
                    }
                }
                else
                {
                    isEmail = false;
                    isValid = true;
                }

                if (isValid)
                {
                    if (!isEmail)
                    {
                        if (!txtUsername.Text.IsValidPhone())
                        {
                            isValid = false;
                            Common.DisplayErrorMessage(Constraints.InValid_PhoneNumber);
                        }
                    }
                    else if (Common.EmptyFiels(txtPassword.Text))
                    {
                        isValid = false;
                        Common.DisplayErrorMessage(Constraints.Required_Password);
                    }
                    else
                    {
                        isValid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/Validations: " + ex.Message);
            }
            return isValid;
        }

        private void RequiredFields()
        {
            try
            {
                if (Common.EmptyFiels(txtUsername.Text))
                {
                    BoxUserName.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
                if (Common.EmptyFiels(txtPassword.Text))
                {
                    BoxPassword.BackgroundColor = (Color)App.Current.Resources["appColor3"];
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/EnableRequiredFields: " + ex.Message);
            }
        }

        private void FieldsTrim()
        {
            txtUsername.Text = txtUsername.Text.Trim();
            txtPassword.Text = txtPassword.Text.Trim();
        }

        private async Task AuthenticateUser()
        {
            try
            {
                if (Validations())
                {
                    FieldsTrim();
                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    Model.Request.Authenticate mAuthenticate = new Model.Request.Authenticate();
                    mAuthenticate.Email = txtUsername.Text;
                    mAuthenticate.Password = txtPassword.Text;
                    if (!Common.EmptyFiels(Settings.fcm_token))
                    {
                        mAuthenticate.FcmToken = Settings.fcm_token;
                    }

                    UserDialogs.Instance.ShowLoading(Constraints.Loading);
                    Response mLocalResponse = new Response();
                    if (isEmail)
                    {
                        mLocalResponse.Data = true;
                    }
                    else
                    {
                        var isValidNumber = await authenticationAPI.CheckPhoneNumberExists(txtUsername.Text);
                        if (isValidNumber != null)
                        {
                            mLocalResponse.Data = isValidNumber.Succeeded;
                            mLocalResponse.Message = isValidNumber.Message;
                        }
                        else
                        {
                            mLocalResponse.Data = false;
                            mLocalResponse.Message = string.Empty;
                        }
                    }

                    if ((bool)mLocalResponse.Data)
                    {
                        var mResponse = await authenticationAPI.SellerAuth(mAuthenticate);
                        mLocalResponse = mResponse;
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

                                    App.Current.MainPage = new MasterData.MasterDataPage();

                                    txtUsername.Text = string.Empty;
                                    txtPassword.Text = string.Empty;
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
                    else
                    {
                        if (mLocalResponse != null && !Common.EmptyFiels(mLocalResponse.Message))
                            Common.DisplayErrorMessage(mLocalResponse.Message);
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

        #region [ Events ]
        private async void StkSignup_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new SignupPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/StkSignup_Tapped: " + ex.Message);
            }
        }

        private async void ForgotPassword_Click(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new ForgotPasswordPage());
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/ForgotPassword_Click: " + ex.Message);
            }
        }

        private async void BtnLogin_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Common.BindAnimation(button: BtnLogin);
                await AuthenticateUser();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/BtnLogin_Tapped: " + ex.Message);
            }
        }

        private void ImgPassword_Tapped(object sender, EventArgs e)
        {
            try
            {
                var selectedImage = Convert.ToString(imgPassword.Source).Replace("File: ", string.Empty);
                if (selectedImage == Constraints.Img_Hide)
                {
                    txtPassword.IsPassword = false;
                    imgPassword.Source = Constraints.Img_Show;
                }
                else
                {
                    txtPassword.IsPassword = true;
                    imgPassword.Source = Constraints.Img_Hide;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/ImgPassword_Tapped: " + ex.Message);
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                var entry = (ExtEntry)sender;
                if (!Common.EmptyFiels(entry.Text))
                {
                    if (entry.ClassId == "Username")
                    {
                        BoxUserName.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                    else if (entry.ClassId == "Password")
                    {
                        BoxPassword.BackgroundColor = (Color)App.Current.Resources["appColor8"];
                    }
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("LoginPage/Entry_Unfocused: " + ex.Message);
            }
        }
        #endregion
    }
}