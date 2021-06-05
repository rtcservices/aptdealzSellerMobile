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
            App.Current.MainPage = new NavigationPage(new WelcomePage());
        }
        #endregion
    }
}