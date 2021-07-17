using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.MasterData;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.MainTabbedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutAptDealzView : ContentView
    {
        #region Objects

        #endregion

        #region Constructor
        public AboutAptDealzView()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        private void ImgMenu_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgNotification_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgQuestion_Tapped(object sender, EventArgs e)
        {

        }

        private void ImgBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
            Common.BindAnimation(imageButton: ImgBack);
            App.Current.MainPage = new MasterDataPage();
        }
        #endregion

        private void BtnLogo_Clicked(object sender, EventArgs e)
        {
            Common.MasterData.Detail = new NavigationPage(new MainTabbedPage("Home"));
        }
    }
}