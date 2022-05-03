using Android.App;
using Android.Content;
using Firebase.Iid;

namespace aptdealzSellerMobile.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            try
            {
                Utility.Settings.fcm_token = FirebaseInstanceId.Instance.Token;
            }catch
            {

            }
        }

        void SendRegistrationToServer(string token)
        {
            // send this token to server
        }
    }
}