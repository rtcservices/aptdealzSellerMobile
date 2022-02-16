﻿using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;

namespace aptdealzSellerMobile.Droid
{
    //You can specify additional application information in this attribute
#if DEBUG
    [Application(Debuggable = true)]
#else
[Application(Debuggable = false)]
#endif
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
             : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);

            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }

            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, new NotificationUserCategory[]
             {
                new NotificationUserCategory("message", new List<NotificationUserAction>
                {
                    new NotificationUserAction("Reply", "Reply", NotificationActionType.Foreground),
                    new NotificationUserAction("Forward", "Forward", NotificationActionType.Foreground)
                }),
                new NotificationUserCategory("request", new List<NotificationUserAction>
                {
                    new NotificationUserAction("Accept", "Accept", NotificationActionType.Default, "check"),
                    new NotificationUserAction("Reject", "Reject", NotificationActionType.Default, "cancel")
                })
             }, false);
#else


	            FirebasePushNotificationManager.Initialize(this,new NotificationUserCategory[]
		    {
			new NotificationUserCategory("message",new List<NotificationUserAction> {
			    new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground),
			    new NotificationUserAction("Forward","Forward",NotificationActionType.Foreground)

			}),
			new NotificationUserCategory("request",new List<NotificationUserAction> {
			    new NotificationUserAction("Accept","Accept",NotificationActionType.Default,"check"),
			    new NotificationUserAction("Reject","Reject",NotificationActionType.Default,"cancel")
			})

		    },false);
#endif

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("NOTIFICATION RECEIVED", p.Data);

                Utility.Notification.ShowNotification($"{p.Data["title"]}", $"{p.Data["body"]}");
            };
        }
    }
}