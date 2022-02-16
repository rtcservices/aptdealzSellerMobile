using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.iOS.DependencService;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Platform;
using Firebase.CloudMessaging;
using Foundation;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using UIKit;
using UserNotifications;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace aptdealzSellerMobile.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            AiForms.Layouts.LayoutsInit.Init();
            CachedImageRenderer.Init();
            FlowListView.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            Rg.Plugins.Popup.Popup.Init();
            CarouselView.FormsPlugin.iOS.CarouselViewRenderer.Init();
            Firebase.Core.App.Configure();

            Plugin.LocalNotification.NotificationCenter.AskPermission();

            LoadApplication(new App());

            FirebasePushNotificationManager.Initialize(options, new NotificationUserCategory[]
            {
                     new NotificationUserCategory("message",new List<NotificationUserAction>
                     {
                         new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground)
                     }),
                     new NotificationUserCategory("request",new List<NotificationUserAction>
                     {
                         new NotificationUserAction("Accept","Accept"),
                         new NotificationUserAction("Reject","Reject",NotificationActionType.Destructive)
                     })
            });

            FirebasePushNotificationManager.Initialize(options, true);
            DependencyService.Register<IFirebaseAuthenticator, FirebaseAuthenticator>();

            // Added by BK 10-14-2021
            //FirebasePushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge | UNNotificationPresentationOptions.Sound; 

            // updated by BK 01-17-2022
            RegisterForNotificationFCM();

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (Xamarin.Essentials.Platform.OpenUrl(app, url, options))
                return true;

            return base.OpenUrl(app, url, options);
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            if (Xamarin.Essentials.Platform.ContinueUserActivity(application, userActivity, completionHandler))
                return true;

            return base.ContinueUserActivity(application, userActivity, completionHandler);
        }

        public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
            => Xamarin.Essentials.Platform.PerformActionForShortcutItem(application, shortcutItem, completionHandler);

        /// <summary>
        /// Code Added By BK 10-13-2021
        /// </summary>
        /// <param name="application"></param>
        /// <param name="deviceToken"></param>
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            try
            {
                FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-RegisteredForRemoteNotifications", ex.Message, "Ok");
            }
        }

        /// <summary>
        /// Code Added By BK 10-13-2021
        /// </summary>
        /// <param name="application"></param>
        /// <param name="error"></param>
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            try
            {
                FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-FailedToRegisterForRemoteNotifications", ex.Message, "Ok");
            }
        }

        /// <summary>
        /// Code Added By BK 10-13-2021
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userInfo"></param>
        /// <param name="completionHandler"></param>
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            try
            {
                FirebasePushNotificationManager.DidReceiveMessage(userInfo);
                completionHandler(UIBackgroundFetchResult.NewData);
                ProcessNotification(userInfo);
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-DidReceiveRemoteNotification", ex.Message, "Ok");
            }
        }

        /// <summary>
        /// Code Added By BK 10-14-2021
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userInfo"></param>
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            if (userInfo == null)
                return;

            ProcessNotification(userInfo);
        }

        /// <summary>
        /// Code Added By BK 10-14-2021
        /// </summary>
        /// <param name="options"></param>
        void ProcessNotification(NSDictionary options)
        {
            try
            {
                if (options != null && options.ContainsKey(new NSString("aps")))
                {
                    string body = string.Empty;
                    string sound = string.Empty;
                    string title = AppInfo.Name;
                    body = (options[new NSString("Message")] as NSString).ToString();
                    //sound = (options[new NSString("Sound")] as NSString).ToString();

                    if (!string.IsNullOrEmpty(body))
                    {

                       // var notification = new UILocalNotification();
                       // notification.SoundName = sound;
                        //  App.PushNotificationForiOS(title, body);
                    }

                    //if (!string.IsNullOrEmpty(sound))
                    //{
                    //    App.Current.MainPage.DisplayAlert("sound", sound, "Ok");
                    //}
                }
            }
            catch (System.Exception ex)
            {
                if (!ex.Message.ToLower().Contains("object reference"))
                    App.Current.MainPage.DisplayAlert("ProcessNotification", ex.Message, "Ok");
            }
        }

        /// <summary>
        /// Code Added By BK 10-14-2021
        /// </summary>
        /// <param name="uiApplication"></param>
        public override void WillEnterForeground(UIApplication uiApplication)
        {
            Plugin.LocalNotification.NotificationCenter.ResetApplicationIconBadgeNumber(uiApplication);
        }

        private void RegisterForNotificationFCM()
        {
            try
            {
                //Firebase Cloud Messaging Configuration
                //Get permission for notification
                if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                {
                    // iOS 10
                    var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                    UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                    {
                        Console.WriteLine(granted);
                    });

                    // For iOS 10 display notification (sent via APNS)
                    UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
                }
                else
                {
                    // iOS 9 <=
                    var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                    var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                    UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
                }
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            catch (Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Exception-RegisterForNotificationFCM", ex.Message, "Ok");
            }
        }
    }
}
