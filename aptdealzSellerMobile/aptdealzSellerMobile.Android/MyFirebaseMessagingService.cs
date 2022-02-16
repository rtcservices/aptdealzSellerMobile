using Android.App;
using Android.Content;
using aptdealzSellerMobile.Droid.DependencService;
using aptdealzSellerMobile.Utility;
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
            try
            {
                base.OnMessageReceived(message);
                if (!Utility.Settings.IsMuteMode)
                {
                    new NotificationHelper().ScheduleNotification(message.GetNotification().Title, message.GetNotification().Body);
                }
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("MyFirebaseMessagingService/OnMessageReceived: " + ex.Message);
            }
        }
    }
}