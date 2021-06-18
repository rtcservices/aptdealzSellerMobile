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
    }
}
