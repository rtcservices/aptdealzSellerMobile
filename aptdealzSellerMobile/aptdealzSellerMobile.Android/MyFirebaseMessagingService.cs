using Android.App;
using Android.Content;
using aptdealzSellerMobile.Droid.DependencService;
using Firebase.Messaging;

namespace aptdealzSellerMobile.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        public MyFirebaseMessagingService()
        {

        }
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            if (!aptdealzSellerMobile.Utility.Settings.IsMuteMode)
            {
                new NotificationHelper().ScheduleNotification(message.GetNotification().Title, message.GetNotification().Body);
            }
        }
    }
}