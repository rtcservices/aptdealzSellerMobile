using Android.App;
using aptdealzSellerMobile.Droid.Implementations;
using aptdealzSellerMobile.Interfaces;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(CloseAppOnBackButton))]

namespace aptdealzSellerMobile.Droid.Implementations
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class CloseAppOnBackButton : ICloseAppOnBackButton, IDisposable
    {
        public void CloseApp()
        {
            try
            {
                var activity = (Activity)Xamarin.Forms.Forms.Context;
                activity.FinishAffinity();
            }
            catch (Exception)
            {
            }
        }

        public void Dispose()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}