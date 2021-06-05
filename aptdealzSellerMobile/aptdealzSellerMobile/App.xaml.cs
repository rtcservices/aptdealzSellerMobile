using aptdealzSellerMobile.Views;
using aptdealzSellerMobile.Views.Dashboard;
using aptdealzSellerMobile.Views.MainTabbedPages;
using aptdealzSellerMobile.Views.OtherPage;
using aptdealzSellerMobile.Views.SplashScreen;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new SplashScreen();

            #region Other Pages
            //MainPage = new MainTabbedPage();
            //MainPage = new RequirementDetailPage();
            //MainPage = new QuoteDetailPage();
            //MainPage = new QuoteEditPage();
            // MainPage = new UpdateOrderDetailPage();
            //MainPage = new GrievancesPage();
            //MainPage = new GrievanceDetailPage();
            //MainPage = new  NotificationPage();
            //MainPage = new  SettingPage();
            //MainPage = new  DeactivateAccountPage();
            //MainPage = new  ContactSupportPage();
            //MainPage = new  WeSupportPage();
            //MainPage = new  CurrentlyShippingPage();
            // MainPage = new  ReportPage();
            //MainPage = new  ReportDetailPage(); 
            #endregion
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
