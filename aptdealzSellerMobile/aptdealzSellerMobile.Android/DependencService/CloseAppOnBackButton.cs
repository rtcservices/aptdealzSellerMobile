using Android.App;
using aptdealzSellerMobile.Droid.DependencService;
using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.Utility;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(CloseAppOnBackButton))]
namespace aptdealzSellerMobile.Droid.DependencService
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CloseAppOnBackButton : ICloseAppOnBackButton, IDisposable
    {
        public void CloseApp(string PageName)
        {
            try
            {
                var activity = (Activity)Xamarin.Forms.Forms.Context;
                activity.SetResult(Result.Canceled);
                activity.FinishAffinity();
            }
            catch (Exception ex)
            {
                Common.DisplayErrorMessage(PageName + "/CloseAppOnBackButton/CloseApp: " + ex.Message);
            }
        }

        public void Dispose()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

    }
#pragma warning restore CS0618 // Type or member is obsolete
}