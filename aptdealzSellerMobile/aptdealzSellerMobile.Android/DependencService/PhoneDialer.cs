using Android.Content;
using Android.Telephony;
using aptdealzSellerMobile.Droid.DependencService;
using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.Utility;
using System.Linq;

[assembly: Xamarin.Forms.Dependency(typeof(PhoneDialer))]

namespace aptdealzSellerMobile.Droid.DependencService
{
    public class PhoneDialer : IDialer
    {
        public bool Dial(string number)
        {
            try
            {
#pragma warning disable CS0618 // Type or member is obsolete
                var context = Xamarin.Forms.Forms.Context;
#pragma warning restore CS0618 // Type or member is obsolete
                if (context == null)
                    return false;

                Intent intent = new Intent(Intent.ActionCall, Android.Net.Uri.Parse("tel:" + number));

                if (IsIntentAvailable(context, intent) == true)
                {
                    context.StartActivity(intent);
                    return true;
                }
                return false;
            }
            catch (System.Exception ex)
            {
                Common.DisplayErrorMessage("PhoneDialer/Dial: " + ex.Message);
                return false;
            }
        }

        public static bool IsIntentAvailable(Context context, Intent intent)
        {

            var packageManager = context.PackageManager;

            var list = packageManager.QueryIntentServices(intent, 0)
                .Union(packageManager.QueryIntentActivities(intent, 0));
            if (list.Any())
                return true;

            TelephonyManager mgr = TelephonyManager.FromContext(context);
            return mgr.PhoneType != PhoneType.None;
        }
    }
}