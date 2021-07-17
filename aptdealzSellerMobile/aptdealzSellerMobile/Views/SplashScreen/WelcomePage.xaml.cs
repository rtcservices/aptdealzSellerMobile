using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aptdealzSellerMobile.Views.SplashScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        #region Objecst
        private List<CarousellImage> mCarousellImages = new List<CarousellImage>();
        #endregion

        #region Ctor
        public WelcomePage()
        {
            InitializeComponent();
            Settings.IsViewWelcomeScreen = false;
        }
        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindCarousallData();
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
                            Xamarin.Forms.DependencyService.Get<ICloseAppOnBackButton>().CloseApp();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("WelcomePage/OnBackButtonPressed: " + ex.Message);
            }
            return true;
        }

        private void BindCarousallData()
        {
            mCarousellImages = new List<CarousellImage>()
            {
                new CarousellImage{ImageName="imgWelcomeOne.png"},
                new CarousellImage{ImageName="imgWelcomeTwo.png"},
                new CarousellImage{ImageName="imgWelcomeThree.png"},
            };
            Indicators.ItemsSource = cvWelcome.ItemsSource = mCarousellImages.ToList();
        }
        #endregion

        #region Events
        private void SkipTapped_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }

        private void BtnLogin_Clicked(object sender, EventArgs e)
        {
            Common.BindAnimation(button: BtnLogin);
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }
        #endregion
    }
}