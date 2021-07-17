using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Services;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.SplashScreen;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace aptdealzSellerMobile
{
    public partial class App : Application
    {
        #region Objects
        public static int latitude = 0;
        public static int longitude = 0;
        #endregion

        public App()
        {
            Device.SetFlags(new string[]
            {
                "MediaElement_Experimental",
                "AppTheme_Experimental"
            });

            InitializeComponent();
            Application.Current.UserAppTheme = OSAppTheme.Light;

            RegisterDependencies();
            GetCurrentLocation();
            MainPage = new SplashScreen();
        }

        public static void RegisterDependencies()
        {
            Xamarin.Forms.DependencyService.Register<IFileUploadRepository, FileUploadRepository>();
            Xamarin.Forms.DependencyService.Register<ICategoryRepository, CategoryRepository>();
            Xamarin.Forms.DependencyService.Register<IProfileRepository, ProfileRepository>();
            Xamarin.Forms.DependencyService.Register<IAuthenticationRepository, AuthenticationRepository>();
            Xamarin.Forms.DependencyService.Register<IOrderRepository, OrderRepository>();
        }

        public async void GetCurrentLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location != null)
                {
                    latitude = (int)location.Latitude;
                    longitude = (int)location.Longitude;
                }
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/GetCurrentLocation: " + ex.Message);
            }
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
