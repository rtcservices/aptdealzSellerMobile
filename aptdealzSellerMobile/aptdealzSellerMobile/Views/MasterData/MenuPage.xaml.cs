using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MasterData
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void Logout_Tapped(object sender, EventArgs e)
        {
            //var isClose = await DisplayAlert(Constraints.Logout, Constraints.AreYouSureWantLogout, Constraints.Yes, Constraints.No);
            //if (isClose)
            //{               
            //    App.Current.MainPage = new NavigationPage(new Views.Accounts.LoginPage());
            //}
        }
    }
}