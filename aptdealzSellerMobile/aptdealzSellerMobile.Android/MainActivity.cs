using Acr.UserDialogs;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Platform;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

namespace aptdealzSellerMobile.Droid
{
    [Activity(Label = "aptdealzSellerMobile", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            CachedImageRenderer.Init(true);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            UserDialogs.Init(this);
            FlowListView.Init();
            Rg.Plugins.Popup.Popup.Init(this);

            CameraPermission();
            GetPermission();

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public async Task CameraPermission()
        {
            try
            {
                string version = Android.OS.Build.VERSION.Release;                         //Android Version No like 4.4.4 etc... 
                var mver = string.Format("{0:0.00}", version.Substring(0, 3));
                double ver = Convert.ToDouble(mver);
                if (ver >= 5.0)
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Camera);
                    if (status != PermissionStatus.Granted)
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Camera);
                        //Best practice to always check that the key exists
                        if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Camera))
                        {
                            status = results[Plugin.Permissions.Abstractions.Permission.Camera];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }

        public void GetPermission()
        {
            //var name = Android.OS.Build.VERSION.SdkInt;     //Android Version Name like Kitkate etc... 
            string version = Android.OS.Build.VERSION.Release;    //Android Version No like 4.4.4 etc... 
            //double ver = ConvertStringToDouble(version);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                const string AccessFineLocationpermission = Manifest.Permission.AccessFineLocation;
                const string AccessCoarseLocationpermission = Manifest.Permission.AccessCoarseLocation;
                const string AccessLocationExtraCommandspermission = Manifest.Permission.AccessLocationExtraCommands;
                const string AccessMockLocationpermission = Manifest.Permission.AccessMockLocation;
                const string AccessNetworkStatepermission = Manifest.Permission.AccessNetworkState;
                const string ChangeWifiStatepermission = Manifest.Permission.ChangeWifiState;
                const string Internetpermission = Manifest.Permission.Internet;
                const string Camerapermission = Manifest.Permission.Camera;
                const string ReadExternalStoragepermission = Manifest.Permission.ReadExternalStorage;
                const string WriteExternalStoragepermission = Manifest.Permission.WriteExternalStorage;

                if (CheckSelfPermission(AccessFineLocationpermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(AccessCoarseLocationpermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(AccessLocationExtraCommandspermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(AccessMockLocationpermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(AccessNetworkStatepermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(ChangeWifiStatepermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(Internetpermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(Camerapermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(ReadExternalStoragepermission) != (int)Android.Content.PM.Permission.Granted
                   || CheckSelfPermission(WriteExternalStoragepermission) != (int)Android.Content.PM.Permission.Granted)
                {
                    RequestPermissions(new string[]  {
                        Manifest.Permission.AccessFineLocation,
                        Manifest.Permission.AccessCoarseLocation,
                        Manifest.Permission.AccessLocationExtraCommands,
                        Manifest.Permission.AccessMockLocation,
                        Manifest.Permission.AccessNetworkState,
                        Manifest.Permission.ChangeWifiState,
                        Manifest.Permission.Internet,
                        Manifest.Permission.Camera,
                        Manifest.Permission.ReadExternalStorage,
                        Manifest.Permission.WriteExternalStorage,
                },
                101);
                }
            }
        }
    }
}