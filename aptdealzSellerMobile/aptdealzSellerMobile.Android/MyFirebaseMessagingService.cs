using Android.App;
using Android.Content;
using aptdealzSellerMobile.Droid.DependencService;
using aptdealzSellerMobile.Utility;
using Firebase.Messaging;
using System;
using Xamarin.Forms;

namespace aptdealzSellerMobile.Droid
{
    [Service(Exported = true, Name = "com.zartek.quotesouk.bidder.MyFirebaseMessagingService")]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public MyFirebaseMessagingService()
        {

        }
        public override void OnMessageReceived(RemoteMessage message)
        {
            try
            {
                base.OnMessageReceived(message);
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (!Utility.Settings.IsMuteMode)
                    {
                        NotificationHelper notificationHelper = new NotificationHelper();
                        notificationHelper.ScheduleNotification(message.GetNotification().Title, message.GetNotification().Body);
                    }
                    MessagingCenter.Send<string>(string.Empty, Constraints.NotificationReceived);
                });
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage("MyFirebaseMessagingService/OnMessageReceived: " + ex.Message);
            }
        }
    }
}