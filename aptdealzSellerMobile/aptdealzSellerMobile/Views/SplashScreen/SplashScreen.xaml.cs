using Acr.UserDialogs;
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
            await Task.Delay(5 * 1000);
            App.Current.MainPage = new NavigationPage(new Views.SplashScreen.WelcomePage());
        }
        #endregion
    }
}