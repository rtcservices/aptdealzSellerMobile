using aptdealzSellerMobile.Repository;
using aptdealzSellerMobile.Services;
using aptdealzSellerMobile.Utility;
using aptdealzSellerMobile.Views.MasterData;
using aptdealzSellerMobile.Views.SplashScreen;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace aptdealzSellerMobile
{
    public partial class App : Application
    {
        #region [ Objects ]
        public static int latitude = 0;
        public static int longitude = 0;
        //public static StoppableTimer stoppableTimer;
        //public static StoppableTimer chatStoppableTimer;
        //public static bool IsNotification = false;
        const string androidKey = "85e79628-3808-4af7-ac74-a0c3c08e5fb6";
        const string LogTag = "AppCenterQuotesoukBidder";
        #endregion

        #region [ Ctor ]
        public App()
        {
            Xamarin.Forms.Device.SetFlags(new string[]
            {
                "MediaElement_Experimental",
                "AppTheme_Experimental",
                "FastRenderers_Experimental",
                "CollectionView_Experimental"
            });
            Crashes.SendingErrorReport += SendingErrorReportHandler;
            Crashes.SentErrorReport += SentErrorReportHandler;
            Crashes.FailedToSendErrorReport += FailedToSendErrorReportHandler;
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
            //GetCurrentLocation();
            BindCrossFirebasePushNotification();

            if (!Settings.IsNotification)
            {
                MainPage = new SplashScreen();
            }
            else
            {
                MainPage = new MasterDataPage();
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
                    if (DeviceInfo.Platform == DevicePlatform.iOS && !Common.EmptyFiels(p.Token))
                    {
                        Utility.Settings.fcm_token = p.Token;
                    }
                };

                CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
                {
                    System.Diagnostics.Debug.WriteLine("Received");
                    MessagingCenter.Send<string>(string.Empty, Constraints.NotificationReceived);
                };

                CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
                {
                    Settings.IsNotification = true;
                    if (Settings.IsNotification)
                    {
                        if (Common.mSellerDetails != null && !Common.EmptyFiels(Common.mSellerDetails.SellerId) && !Common.EmptyFiels(Common.Token))
                        {
                            MainPage = new MasterDataPage();
                        }
                        else
                        {
                            MainPage = new Views.SplashScreen.SplashScreen();
                        }
                    }
                    Settings.IsNotification = false;
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
        static void SendingErrorReportHandler(object sender, SendingErrorReportEventArgs e)
        {
            AppCenterLog.Info(LogTag, "Sending error report");

            var args = e as SendingErrorReportEventArgs;
            ErrorReport report = args.Report;

            //test some values
            if (report.StackTrace != null)
            {
                AppCenterLog.Info(LogTag, report.StackTrace.ToString());
            }
            else if (report.AndroidDetails != null)
            {
                AppCenterLog.Info(LogTag, report.AndroidDetails.ThreadName);
            }
        }

        static void SentErrorReportHandler(object sender, SentErrorReportEventArgs e)
        {
            AppCenterLog.Info(LogTag, "Sent error report");

            var args = e as SentErrorReportEventArgs;
            ErrorReport report = args.Report;

            //test some values
            if (report.StackTrace != null)
            {
                AppCenterLog.Info(LogTag, report.StackTrace.ToString());
            }
            else
            {
                AppCenterLog.Info(LogTag, "No system exception was found");
            }

            if (report.AndroidDetails != null)
            {
                AppCenterLog.Info(LogTag, report.AndroidDetails.ThreadName);
            }
        }

        static void FailedToSendErrorReportHandler(object sender, FailedToSendErrorReportEventArgs e)
        {
            AppCenterLog.Info(LogTag, "Failed to send error report");

            var args = e as FailedToSendErrorReportEventArgs;
            ErrorReport report = args.Report;

            //test some values
            if (report.StackTrace != null)
            {
                AppCenterLog.Info(LogTag, report.StackTrace.ToString());
            }
            else if (report.AndroidDetails != null)
            {
                AppCenterLog.Info(LogTag, report.AndroidDetails.ThreadName);
            }

            if (e.Exception != null)
            {
                AppCenterLog.Info(LogTag, "There is an exception associated with the failure");
            }
        }
        protected override void OnStart()
        {
            AppCenter.LogLevel = LogLevel.Verbose;
            Crashes.ShouldProcessErrorReport = ShouldProcess;
            //Crashes.ShouldAwaitUserConfirmation = ConfirmationHandler;
            //Crashes.GetErrorAttachments = GetErrorAttachments;
            AppCenter.Start($"android={androidKey}", typeof(Analytics), typeof(Crashes));

            AppCenter.GetInstallIdAsync().ContinueWith(installId =>
            {
                AppCenterLog.Info(LogTag, "AppCenter.InstallId=" + installId.Result);
            });
            Crashes.HasCrashedInLastSessionAsync().ContinueWith(hasCrashed =>
            {
                AppCenterLog.Info(LogTag, "Crashes.HasCrashedInLastSession=" + hasCrashed.Result);
            });
            Crashes.GetLastSessionCrashReportAsync().ContinueWith(report =>
            {
                AppCenterLog.Info(LogTag, "Crashes.LastSessionCrashReport.StackTrace=" + report.Result?.StackTrace);
            });
        }
        bool ShouldProcess(ErrorReport report)
        {
            AppCenterLog.Info(LogTag, "Determining whether to process error report");
            return true;
        }
        bool ConfirmationHandler()
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                Current.MainPage.DisplayActionSheet("Crash detected. Send anonymous crash report?", null, null, "Send", "Always Send", "Don't Send").ContinueWith((arg) =>
                {
                    var answer = arg.Result;
                    UserConfirmation userConfirmationSelection;
                    if (answer == "Send")
                    {
                        userConfirmationSelection = UserConfirmation.Send;
                    }
                    else if (answer == "Always Send")
                    {
                        userConfirmationSelection = UserConfirmation.AlwaysSend;
                    }
                    else
                    {
                        userConfirmationSelection = UserConfirmation.DontSend;
                    }
                    AppCenterLog.Debug(LogTag, "User selected confirmation option: \"" + answer + "\"");
                    Crashes.NotifyUserConfirmation(userConfirmationSelection);
                });
            });

            return true;
        }

        IEnumerable<ErrorAttachmentLog> GetErrorAttachments(ErrorReport report)
        {
            return new ErrorAttachmentLog[]
            {
                ErrorAttachmentLog.AttachmentWithText("Hello world!", "hello.txt"),
                ErrorAttachmentLog.AttachmentWithBinary(Encoding.UTF8.GetBytes("Fake image"), "fake_image.jpeg", "image/jpeg")
            };
        }

        protected override void OnSleep()
        {
            //if (App.stoppableTimer != null)
            //    stoppableTimer.Stop();
        }

        protected override void OnResume()
        {
            //if (App.stoppableTimer != null)
            //    stoppableTimer.Start();
        }
        #endregion
    }
}
