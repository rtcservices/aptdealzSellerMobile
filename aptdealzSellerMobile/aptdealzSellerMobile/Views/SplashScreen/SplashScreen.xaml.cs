using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.MasterData;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.SplashScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreen : ContentPage
    {
        #region [ Ctor ]
        public SplashScreen()
        {
            InitializeComponent();
        }
        #endregion

        #region [ Method ]
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

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(5 * 1000);
            BindNavigation();
        }

        private void BindNavigation()
        {
            try
            {
                if (Common.EmptyFiels(Settings.UserToken))
                {
                    if (Settings.IsViewWelcomeScreen)
                    {
                        App.Current.MainPage = new NavigationPage(new Views.SplashScreen.WelcomePage());
                    }
                    else
                    {
                        App.Current.MainPage = new NavigationPage(new Views.Accounts.LoginPage());
                    }
                }
                else
                {
                    Common.Token = Settings.UserToken;
                    App.Current.MainPage = new MasterDataPage();
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