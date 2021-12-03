﻿using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Services;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.SplashScreen;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace aptdealzSellerMobile
{
    public partial class App : Application
    {
        #region [ Objects ]
        public static int latitude = 0;
        public static int longitude = 0;
        public static StoppableTimer stoppableTimer;
        public static bool IsNotification = false;
        public static StoppableTimer chatStoppableTimer;
        #endregion

        #region [ Ctor ]
        public App()
        {
            Device.SetFlags(new string[]
            {
                "MediaElement_Experimental",
                "AppTheme_Experimental"
            });

            InitializeComponent();
            if (Settings.IsDarkMode)
            {
                Application.Current.UserAppTheme = OSAppTheme.Dark;
            }
            else
            {
                Application.Current.UserAppTheme = OSAppTheme.Light;
            }

            RegisterDependencies();
            GetCurrentLocation();
            BindCrossFirebasePushNotification();

            if (!IsNotification)
            {
                MainPage = new SplashScreen();
            }
            else
            {
                MainPage = new Views.MasterData.MasterDataPage(true);
                IsNotification = false;
            }
        }
        #endregion

        #region [ Methods ]
        public static void RegisterDependencies()
        {
            Xamarin.Forms.DependencyService.Register<IFileUploadRepository, FileUploadRepository>();
            Xamarin.Forms.DependencyService.Register<IProfileRepository, ProfileRepository>();
            Xamarin.Forms.DependencyService.Register<IAuthenticationRepository, AuthenticationRepository>();
            Xamarin.Forms.DependencyService.Register<IRequirementRepository, RequirementRepository>();
            Xamarin.Forms.DependencyService.Register<IOrderRepository, OrderRepository>();
            Xamarin.Forms.DependencyService.Register<IGrievanceRepository, GrievanceRepository>();
            Xamarin.Forms.DependencyService.Register<INotificationRepository, NotificationRepository>();
        }

        private async void GetCurrentLocation()
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
                if (!ex.Message.Contains("Denied"))
                {
                    Common.DisplayErrorMessage("App/GetCurrentLocation: " + ex.Message);
                }
            }
        }

        private void BindCrossFirebasePushNotification()
        {
            try
            {
                CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        Utility.Settings.fcm_token = p.Token;
                    }
                };

                CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    MainPage = new Views.MasterData.MasterDataPage(true);
                };

                CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
                {
                    IsNotification = true;
                };

                CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Action");

                    if (!string.IsNullOrEmpty(p.Identifier))
                    {
                        System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
                        foreach (var data in p.Data)
                        {
                            System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                        }
                    }
                };

                CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Deleted");
                };
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("App/BindCrossFirebasePushNotification: " + ex.Message);
            }
        }

        public static void PushNotificationForiOS(string title, string message)
        {
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    var notification = new NotificationRequest
                    {
                        NotificationId = 100,
                        Title = title,
                        Description = message,
                        BadgeNumber = 1,
                    };
                    NotificationCenter.Current.Show(notification);
                }
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("App/PushNotificationForiOS: " + ex.Message);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            if (App.stoppableTimer != null)
                stoppableTimer.Stop();
        }

        protected override void OnResume()
        {
            if (App.stoppableTimer != null)
                stoppableTimer.Start();
        }
        #endregion
    }
}
