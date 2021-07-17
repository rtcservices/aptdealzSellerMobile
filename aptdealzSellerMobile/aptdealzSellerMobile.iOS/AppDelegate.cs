using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Platform;
using Foundation;
using UIKit;

namespace aptdealzSellerMobile.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            AiForms.Layouts.LayoutsInit.Init();
            CachedImageRenderer.Init();
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
            CarouselView.FormsPlugin.iOS.CarouselViewRenderer.Init();
            FlowListView.Init();
            Rg.Plugins.Popup.Popup.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (Xamarin.Essentials.Platform.OpenUrl(app, url, options))
                return true;

            return base.OpenUrl(app, url, options);
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            if (Xamarin.Essentials.Platform.ContinueUserActivity(application, userActivity, completionHandler))
                return true;

            return base.ContinueUserActivity(application, userActivity, completionHandler);
        }

        public override void PerformActionForShortcutItem(UIApplication application, UIApplicationShortcutItem shortcutItem, UIOperationHandler completionHandler)
            => Xamarin.Essentials.Platform.PerformActionForShortcutItem(application, shortcutItem, completionHandler);

    }
}
