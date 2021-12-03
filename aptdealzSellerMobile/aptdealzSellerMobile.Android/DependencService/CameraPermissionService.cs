using Android;
using Android.Content;
using aptdealzSellerMobile.Droid.DependencService;
using aptdealzSellerMobile.Interfaces;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(CameraPermissionService))]
namespace aptdealzSellerMobile.Droid.DependencService
{
    public class CameraPermissionService : ICameraPermission
    {
        public void CameraPermission()
        {
            try
            {
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    var mainActivity = (MainActivity)Xamarin.Forms.Forms.Context;

                    const string Camerapermission = Manifest.Permission.Camera;

                    if (mainActivity.CheckSelfPermission(Camerapermission) != (int)Android.Content.PM.Permission.Granted)
                    {
                        Intent intent = new Intent(Android.Provider.Settings.ActionApplicationDetailsSettings);
                        intent.AddFlags(ActivityFlags.NewTask);

                        Android.Net.Uri uri = Android.Net.Uri.Parse("package:" + Xamarin.Forms.Forms.Context.PackageName);
                        intent.SetData(uri);

                        mainActivity.StartActivityForResult(intent, 101);
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }
    }
}