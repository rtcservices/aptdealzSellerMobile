using aptdealzSellerMobile.API;
using aptdealzSellerMobile.Utility;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.SplashScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        #region Method
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(5 * 1000);
            BindNavigation();
        }

        async void BindNavigation()
        {
            try
            {
                await Task.Delay(5 * 1000);

                if (Common.EmptyFiels(Settings.UserToken))
                {
                    App.Current.MainPage = new NavigationPage(new WelcomePage());
                }
                else
                {
                    Common.Token = Settings.UserToken;

                    AuthenticationAPI authenticationAPI = new AuthenticationAPI();
                    var mResponse = await authenticationAPI.RefreshToken(Settings.RefreshToken);
                    if (mResponse != null && mResponse.Succeeded)
                    {
                        var jObject = (Newtonsoft.Json.Linq.JObject)mResponse.Data;
                        if (jObject != null)
                        {
                            var mSeller = jObject.ToObject<Model.Reponse.Seller>();
                            if (mSeller != null)
                            {
                                Settings.UserId = mSeller.Id;
                                Settings.UserToken = mSeller.JwToken;
                                Common.Token = mSeller.JwToken;
                                Settings.RefreshToken = mSeller.RefreshToken;
                                Settings.LoginTrackingKey = mSeller.LoginTrackingKey == "00000000-0000-0000-0000-000000000000" ? Settings.LoginTrackingKey : mSeller.LoginTrackingKey;
                                App.Current.MainPage = new MainTabbedPages.MainTabbedPage("Home");
                            }
                        }
                    }
                    else
                    {
                        if (mResponse != null)
                            Common.DisplayErrorMessage(mResponse.Message);
                        else
                            Common.DisplayErrorMessage(Constraints.Something_Wrong);

                        App.Current.MainPage = new NavigationPage(new WelcomePage());
                    }
                }
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("Spalshscreen/BindNavigation: " + ex.Message);
            }
        }
        #endregion
    }
}