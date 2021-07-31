using aptdealzSellerMobile.Interfaces;
using aptdealzSellerMobile.iOS.DependencService;
using Foundation;
using System;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(PhoneDialer))]
namespace aptdealzSellerMobile.iOS.DependencService
{
    public class PhoneDialer : IDialer
    {
        public bool Dial(string number)
        {
            try
            {
                return UIApplication.SharedApplication.OpenUrl(new NSUrl("tel:" + number)); ;
            }
            catch (Exception ex)
            {
                var errormsg = ex.Message;
                return false;
            }
        }
    }
}