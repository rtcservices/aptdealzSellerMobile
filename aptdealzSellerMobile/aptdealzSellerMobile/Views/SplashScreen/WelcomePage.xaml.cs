using aptdealzSellerMobile.Model;
using aptdealzSellerMobile.Views.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public WelcomePage()
        {
            InitializeComponent();
        }

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindCarousallData();
        }

        private void BindCarousallData()
        {
            mCarousellImages = new List<CarousellImage>()
            {
                new CarousellImage{ImageName="imgWelcomeOne.png"},
                new CarousellImage{ImageName="imgWelcomeTwo.png"},
                new CarousellImage{ImageName="imgWelcomeThree.png"},
            };
            indicaters.ItemsSource = cvWelcome.ItemsSource = mCarousellImages.ToList();
        }
        #endregion

        private async void Loginbtn_Click(object sender, EventArgs e)
        {
            await btnLoginTapped.ScaleTo(0.9, 100, Easing.Linear);
            await btnLoginTapped.ScaleTo(1.0, 100, Easing.Linear);
            await Navigation.PushAsync(new LoginPage());
        }

        private void SkipTapped_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }
    }
}